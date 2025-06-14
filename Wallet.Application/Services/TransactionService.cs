using AutoMapper;
using Wallet.Application.Dto;
using Wallet.Application.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionTagRepository _transactionTagRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, ITransactionRepository transactionRepository, ITransactionTagRepository transactionTagRepository, IAccountRepository accountRepository, ITagRepository tagRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _transactionTagRepository = transactionTagRepository;
        _accountRepository = accountRepository;
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDtoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        //Докинуть тег 
        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);
        
        if (transaction == null)
        {
            throw new ArgumentException("Не найдено транзакций");
        }
        
        var transactionDto = _mapper.Map<TransactionDtoResponse>(transaction);

        return transactionDto;    
    }
    

    public async Task<IEnumerable<TransactionDtoResponse>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetByAccountIdAsync(accountId, cancellationToken);
        
        if (!transactions.Any())
        {
            return Enumerable.Empty<TransactionDtoResponse>();
        }
        
        return _mapper.Map<IEnumerable<TransactionDtoResponse>>(transactions);
    }


    public async Task<Guid> CreateAsync(TransactionDto transaction, CancellationToken cancellationToken)
    {
        // Получаем аккаунт
        var account = await _unitOfWork.AccountRepository.GetByIdAsync(transaction.AccountId, cancellationToken);
        
        if (account == null)
        {
            throw new ArgumentException("Не найден аккаунт");
        }

        if (transaction.OperationType == OperationType.Income)
        {
            account.CurrentBalance += transaction.Amount;
        }
        else if (transaction.OperationType == OperationType.Expense)
        {
            account.CurrentBalance -= transaction.Amount;
        }
        
        account.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.AccountRepository.UpdateAsync(account, cancellationToken);
        
        // Получаем или создаем тег
        var tag = await _unitOfWork.TagRepository.GetTagIdByNameAsync(transaction.Tag, account.UserId, cancellationToken);

        if (tag == null)
        {
            var newTag = new Tag
            {
                Name = transaction.Tag,
                UserId = account.UserId
            };

            await _unitOfWork.TagRepository.CreateAsync(newTag, cancellationToken);
            tag = newTag;
        }
        
        // Создаем транзакцию
        var newTransaction = _mapper.Map<Transaction>(transaction);
        
        await _unitOfWork.TransactionRepository.AddAsync(newTransaction, cancellationToken);

        // Создаем связь транзакции с тегом
        var transactionTag = new TransactionTag
        {
            TransactionId = newTransaction.TransactionId,
            TagId = tag.TagId
        };
        await _unitOfWork.TransactionTagRepository.AddAsync(transactionTag, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newTransaction.TransactionId;
    }

    public async Task UpdateAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken)
    {   
         // Получаем существующую транзакцию
    var existingTransaction = await _unitOfWork.TransactionRepository
        .GetByIdAsync(id, cancellationToken);

    if (existingTransaction == null)
    {
        throw new ArgumentException("Транзакция не найдена");
    }

    // Получаем аккаунт
    var account = await _unitOfWork.AccountRepository
        .GetByIdAsync(transactionDto.AccountId, cancellationToken);

    if (account == null)
    {
        throw new ArgumentException("Аккаунт не найден");
    }

    // Отменяем предыдущее влияние транзакции на баланс
    if (existingTransaction.OperationType == OperationType.Income)
    {
        account.CurrentBalance -= existingTransaction.Amount;
    }
    else if (existingTransaction.OperationType == OperationType.Expense)
    {
        account.CurrentBalance += existingTransaction.Amount;
    }
    
    if (transactionDto.OperationType == OperationType.Income) 
    {
        account.CurrentBalance += transactionDto.Amount;
    }
    else if (transactionDto.OperationType == OperationType.Expense) 
    {
        account.CurrentBalance -= transactionDto.Amount;
    }

    account.UpdatedAt = DateTime.UtcNow;
    await _unitOfWork.AccountRepository.UpdateAsync(account, cancellationToken);

    // Обновляем тег
    var tag = await _unitOfWork.TagRepository
        .GetTagIdByNameAsync(transactionDto.Tag, account.UserId, cancellationToken);

    if (tag == null)
    {
        tag = new Tag
        {
            Name = transactionDto.Tag,
            UserId = account.UserId
        };
        await _unitOfWork.TagRepository.CreateAsync(tag, cancellationToken);
    }

    // Обновляем саму транзакцию
    _mapper.Map(transactionDto, existingTransaction);
    await _unitOfWork.TransactionRepository.UpdateAsync(existingTransaction, cancellationToken);

    // Обновляем связь с тегом
    var existingTransactionTag = await _unitOfWork.TransactionTagRepository
        .GetByTransactionIdAsync(id, cancellationToken);

    if (existingTransactionTag != null)
    {
        existingTransactionTag.TagId = tag.TagId;
        await _unitOfWork.TransactionTagRepository.UpdateAsync(existingTransactionTag, cancellationToken);
    }
    else
    {
        var newTransactionTag = new TransactionTag
        {
            TransactionId = id,
            TagId = tag.TagId
        };
    
        await _unitOfWork.TransactionTagRepository.AddAsync(newTransactionTag, cancellationToken);
    }
    
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);

        if (transaction == null)
            throw new ArgumentException("Транзакция не найдена");

        var account = await _accountRepository.GetByIdAsync(transaction.AccountId, cancellationToken);

        if (account == null)
            throw new ArgumentException("Аккаунт не найден");

        // Отменяем влияние транзакции на баланс
        if (transaction.OperationType == OperationType.Income)
        {
            account.CurrentBalance -= transaction.Amount;
        }
        else if (transaction.OperationType == OperationType.Expense)
        {
            account.CurrentBalance += transaction.Amount;
        }

        account.UpdatedAt = DateTime.UtcNow;
        transaction.IsDeleted = true;
        transaction.UpdatedAt = DateTime.UtcNow;

      
        await _unitOfWork.AccountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.TransactionRepository.UpdateAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) 
        => _transactionRepository.ExistsAsync(id, cancellationToken);
}