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
    private readonly IMapper _mapper;
    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, ITagService tagService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult<TransactionDtoResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id, cancellationToken, true, true);

            if (transaction == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.NotFound,
                    $"Транзакция не найдена: {id}");

            var transactionDto = _mapper.Map<TransactionDtoResponse>(transaction);

            return OperationResult<TransactionDtoResponse>.Success(
                result: transactionDto,
                message:"Ok"
            );
        }
        catch (Exception ex)
        {
            return OperationResult<TransactionDtoResponse>.FromException(ex);
        }
    }
    
    public async Task<OperationResult<IEnumerable<TransactionDtoResponse>>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetByAccountIdAsync(accountId, cancellationToken);

        if (!transactions.Any())
        {
            return OperationResult<IEnumerable<TransactionDtoResponse>>.Success(
                result: Enumerable.Empty<TransactionDtoResponse>(),
                message: "Транзакции не найдены"
            );
        }

        var transactionsDto = _mapper.Map<IEnumerable<TransactionDtoResponse>>(transactions);

        return OperationResult<IEnumerable<TransactionDtoResponse>>.Success(
            result: transactionsDto,
            message:"Ok"
        );
    }
    
    public async Task<OperationResult<TransactionDtoResponse>> CreateAsync(TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(
                transactionDto.AccountId, 
                cancellationToken, 
                noTracking: false);

            if (account == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.NotFound, 
                    $"Аккаунт не найден: {transactionDto.AccountId}");
            
            if (!Enum.IsDefined(typeof(OperationType), transactionDto.OperationType))
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.BadRequest, 
                    $"Недопустимый тип операции: {transactionDto.OperationType}");
            
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(transactionDto.CategoryId, cancellationToken);
            if (category == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.BadRequest, 
                    $"Категория не найдена: {transactionDto.CategoryId}");
            
            // Обновляем баланс
            account.CurrentBalance = CalculateUpdatedBalance(
                account.CurrentBalance,
                transactionDto.Amount,
                transactionDto.OperationType,
                isAdding: true);

            // Работа с тегом
            var tag = await _unitOfWork.TagRepository.GetUserTagByNameAsync(
                transactionDto.Tag, 
                account.UserId, 
                cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = transactionDto.Tag, UserId = account.UserId };
                await _unitOfWork.TagRepository.CreateAsync(tag, cancellationToken);
            }

            // Создаем и заполняем транзакцию
            var newTransaction = _mapper.Map<Transaction>(transactionDto);
            //newTransaction.TransactionDate = DateTime.UtcNow;
            newTransaction.TagId = tag.TagId;
            
            await _unitOfWork.TransactionRepository.AddAsync(newTransaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            var responseDto = _mapper.Map<TransactionDtoResponse>(newTransaction);
            responseDto.CategoryName = category.Name;
            responseDto.UserId = account.UserId;
            responseDto.TagName = tag.Name;
            return OperationResult<TransactionDtoResponse>.Success(
                result: responseDto,
                message: "Транзакция успешно создана"
            );
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return OperationResult<TransactionDtoResponse>.FromException(ex);
        }
    }

    public async Task<OperationResult<TransactionDtoResponse>> UpdateAsync(
        Guid id,
        TransactionDto transactionDto,
        CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var existingTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(
                id,
                cancellationToken,
                includeRelated: true,
                noTracking: false);

            if (existingTransaction == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.NotFound, 
                    $"Транзакция не найдена: {id}");
            
            if (!Enum.IsDefined(typeof(OperationType), transactionDto.OperationType))
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.BadRequest, 
                    $"Недопустимый тип операции: {transactionDto.OperationType}");

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(
                transactionDto.CategoryId, 
                cancellationToken);

            if (category == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.BadRequest, 
                    $"Категория не найдена: {transactionDto.CategoryId}");
            
            var account = existingTransaction.Account;
            
            account.CurrentBalance = CalculateUpdatedBalance(
                account.CurrentBalance,
                existingTransaction.Amount,
                existingTransaction.OperationType,
                isAdding: false);
            
            account.CurrentBalance = CalculateUpdatedBalance(
                account.CurrentBalance,
                transactionDto.Amount,
                transactionDto.OperationType,
                isAdding: true);
            
            Tag? tag = null;
            if (!string.IsNullOrEmpty(transactionDto.Tag))
            {
                tag = await _unitOfWork.TagRepository.GetUserTagByNameAsync(
                    transactionDto.Tag, 
                    account.UserId, 
                    cancellationToken);

                if (tag == null)
                {
                    tag = new Tag { Name = transactionDto.Tag, UserId = account.UserId };
                    await _unitOfWork.TagRepository.CreateAsync(tag, cancellationToken);
                }
            }
            
            existingTransaction.TagId = tag?.TagId;
            existingTransaction.CategoryId = category.CategoryId;
            existingTransaction.Amount = transactionDto.Amount;
            existingTransaction.OperationType = transactionDto.OperationType;
            existingTransaction.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            var responseDto = _mapper.Map<TransactionDtoResponse>(existingTransaction);
            responseDto.CategoryName = category.Name;

            return OperationResult<TransactionDtoResponse>.Success(
                result: responseDto,
                message: "Транзакция успешно обновлена"
            );
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return OperationResult<TransactionDtoResponse>.FromException(ex);
        }
    }

    public async Task<OperationResult<DeleteTransactionDto>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(
                id, 
                cancellationToken,
                includeRelated: true,
                noTracking: false);

            if (transaction == null)
                return OperationResult<DeleteTransactionDto>.Failure(
                    Enum_StatusCode.NotFound, $"Транзакция не найдена: {id}");
            
            var account = transaction.Account;
            
            account.CurrentBalance = CalculateUpdatedBalance(account.CurrentBalance, transaction.Amount, transaction.OperationType, false);
            transaction.IsDeleted = true;
            transaction.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            var responseDto = _mapper.Map<DeleteTransactionDto>(transaction);
            return OperationResult<DeleteTransactionDto>.Success(
                result: responseDto,
                message: "Транзакция успешно удалена"
            );
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return OperationResult<DeleteTransactionDto>.FromException(ex);
        }
    }
    
    private decimal CalculateUpdatedBalance(decimal currentBalance, decimal amount, OperationType operationType, bool isAdding)
    {
        if (operationType == OperationType.Income)
            return isAdding ? currentBalance + amount : currentBalance - amount;
        else
            return isAdding ? currentBalance - amount : currentBalance + amount;
    }
}