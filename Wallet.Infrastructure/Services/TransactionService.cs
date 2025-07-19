using AutoMapper;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Helpers;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Application.Interfaces.Services;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;

namespace Wallet.Infrastructure.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, ITransactionRepository transactionRepository, IAccountRepository accountRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
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


    public async Task<OperationResult<Guid>> CreateAsync(TransactionDto transaction, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(transaction.AccountId, cancellationToken);
            if (account == null)
                return OperationResult<Guid>.Failure(Enum_StatusCode.NotFound, $"Аккаунт не найден: {transaction.AccountId}");

            // Обновляем баланс
            if (transaction.OperationType == OperationType.Income)
            {
                account.CurrentBalance += transaction.Amount;
            }
            else if (transaction.OperationType == OperationType.Expense)
            {
                account.CurrentBalance -= transaction.Amount;
            }
            else
            {
                return OperationResult<Guid>.Failure(Enum_StatusCode.NotFound, $"Вид поступлений не найден: {transaction.OperationType}");
            }
            
            account.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.AccountRepository.UpdateAsync(account, cancellationToken);

            // Работа с тегом
            var tag = await _unitOfWork.TagRepository.GetUserTagByNameAsync(transaction.Tag, account.UserId, cancellationToken);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = transaction.Tag,
                    UserId = account.UserId
                };
                await _unitOfWork.TagRepository.CreateAsync(tag, cancellationToken);
            }

            // Создаем транзакцию
            var newTransaction = _mapper.Map<Transaction>(transaction);
            await _unitOfWork.TransactionRepository.AddAsync(newTransaction, cancellationToken);

            // Связь транзакции с тегом
            var transactionTag = new TransactionTag
            {
                TransactionId = newTransaction.TransactionId,
                TagId = tag.TagId
            };
            await _unitOfWork.TransactionTagRepository.AddAsync(transactionTag, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<Guid>.Success(
                Enum_StatusCode.OK,
                newTransaction.TransactionId,  // Добавлен ID транзакции
                "Транзакция успешно создана");
        }
        catch (Exception ex)
        {
            return OperationResult<Guid>.FromException(ex);
        }
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
        .GetUserTagByNameAsync(transactionDto.Tag, account.UserId, cancellationToken);

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

    public async Task<OperationResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);
            if (transaction == null)
                return OperationResult.Failure(Enum_StatusCode.NotFound, $"Транзакция не найдена id : {id}");

            var account = await _accountRepository.GetByIdAsync(transaction.AccountId, cancellationToken);
            if (account == null)
                return OperationResult.Failure(Enum_StatusCode.NotFound, "Аккаунт не найден");

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

            return OperationResult.Success(Enum_StatusCode.OK, "Транзакция успешно удалена");
        }
        catch (Exception ex)
        {
            return OperationResult.FromException(ex);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) 
        => _transactionRepository.ExistsAsync(id, cancellationToken);
}