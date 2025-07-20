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
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, ITransactionRepository transactionRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDtoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        //Докинуть тег 
        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken, true, true );
        
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


    public async Task<OperationResult<TransactionDtoResponse>> CreateAsync(TransactionDto transactionDto, CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            // Получаем аккаунт с отслеживанием изменений
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(
                transactionDto.AccountId, 
                cancellationToken, 
                noTracking: false);

            if (account == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.NotFound, 
                    $"Аккаунт не найден: {transactionDto.AccountId}");
            
            // Валидация типа операции
            if (!Enum.IsDefined(typeof(OperationType), transactionDto.OperationType))
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.BadRequest, 
                    $"Недопустимый тип операции: {transactionDto.OperationType}");
            
            // Обновляем баланс
            account.CurrentBalance = transactionDto.OperationType == OperationType.Income
                ? account.CurrentBalance + transactionDto.Amount
                : account.CurrentBalance - transactionDto.Amount;
            
            account.UpdatedAt = DateTime.UtcNow;

            // Работа с тегом
            var tag = await _unitOfWork.TagRepository.GetUserTagByNameAsync(
                transactionDto.Tag, 
                account.UserId, 
                cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = transactionDto.Tag, UserId = account.UserId };
                await _unitOfWork.TagRepository.CreateAsync(tag, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken); // Сохраняем, чтобы получить ID
            }

            // Создаем и заполняем транзакцию
            var newTransaction = _mapper.Map<Transaction>(transactionDto);
            newTransaction.TransactionDate = DateTime.UtcNow;
            
            await _unitOfWork.TransactionRepository.AddAsync(newTransaction, cancellationToken);
            
            // Создаем связь с тегом
            await _unitOfWork.TransactionTagRepository.AddAsync(new TransactionTag
            {
                TransactionId = newTransaction.TransactionId,
                TagId = tag.TagId
            }, cancellationToken);

            // Сохраняем все изменения
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            // Маппим к DTO и возвращаем
            var responseDto = _mapper.Map<TransactionDtoResponse>(newTransaction);
            return OperationResult<TransactionDtoResponse>.Success(
                result: responseDto,
                message: "Транзакция успешно создана"
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return OperationResult<TransactionDtoResponse>.FromException(ex);
        }
    }

   public async Task<OperationResult<TransactionDtoResponse>> UpdateAsync(
    Guid id,
    TransactionDto transactionDto,
    CancellationToken cancellationToken)
    {
        await using var dbTransaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var existingTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(
                id,
                cancellationToken,
                includeRelated: true,
                noTracking: false);

            if (existingTransaction == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.NotFound, $"Транзакция не найдена: {id}");

            if (existingTransaction.Account == null)
                return OperationResult<TransactionDtoResponse>.Failure(
                    Enum_StatusCode.NotFound, "Связанный аккаунт не найден");

            // баланс
            var account = existingTransaction.Account;
            account.CurrentBalance = existingTransaction.OperationType == OperationType.Income
                ? account.CurrentBalance - existingTransaction.Amount
                : account.CurrentBalance + existingTransaction.Amount;
            account.UpdatedAt = DateTime.UtcNow;
            
            existingTransaction.Amount = transactionDto.Amount;
            existingTransaction.CategoryId = transactionDto.CategoryId;
            existingTransaction.Comment = transactionDto.Comment;
            existingTransaction.OperationType = transactionDto.OperationType;

            // Новый баланс
            account.CurrentBalance = transactionDto.OperationType == OperationType.Income
                ? account.CurrentBalance + transactionDto.Amount
                : account.CurrentBalance - transactionDto.Amount;
            existingTransaction.UpdatedAt = DateTime.UtcNow;
            
            var newTagName = transactionDto.Tag?.Trim();
            var transactionTag = existingTransaction.TransactionTags.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(newTagName))
            {
                var tag = await _unitOfWork.TagRepository.GetUserTagByNameAsync(
                    newTagName, account.UserId, cancellationToken);

                if (tag == null)
                {
                    tag = new Tag { Name = newTagName, UserId = account.UserId };
                    await _unitOfWork.TagRepository.CreateAsync(tag, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken); 
                }

                if (transactionTag != null)
                {
                    if (transactionTag.TagId != tag.TagId)
                    {
                        transactionTag.TagId = tag.TagId;
                    }
                }
                else
                {
                    await _unitOfWork.TransactionTagRepository.AddAsync(
                        new TransactionTag
                        {
                            TransactionId = existingTransaction.TransactionId,
                            TagId = tag.TagId
                        }, cancellationToken);
                }
            }
            else
            {
                if (transactionTag != null)
                {
                    _unitOfWork.TransactionTagRepository.DeleteAsync(transactionTag.TransactionId, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);

            // Получаем обновлённую сущность для ответа
            var updatedTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(
                id, cancellationToken, includeRelated: true, noTracking: true);

            var responseDto = _mapper.Map<TransactionDtoResponse>(updatedTransaction);
            return OperationResult<TransactionDtoResponse>.Success(
                result: responseDto,
                message: "Транзакция успешно создана"
            );
        }
        catch (Exception ex)
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            return OperationResult<TransactionDtoResponse>.FromException(ex);
        }
    }
    public async Task<OperationResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            // Получаем транзакцию с отслеживанием и включенными зависимостями
            var transaction = await _transactionRepository.GetByIdAsync(
                id, 
                cancellationToken,
                includeRelated: true,
                noTracking: false);

            if (transaction == null)
                return OperationResult.Failure(Enum_StatusCode.NotFound, $"Транзакция не найдена id: {id}");
            
            var account = transaction.Account;
            
            if (transaction.OperationType == OperationType.Income)
                account.CurrentBalance -= transaction.Amount;
            else if (transaction.OperationType == OperationType.Expense)
                account.CurrentBalance += transaction.Amount;
            
            var now = DateTime.UtcNow;
            account.UpdatedAt = now;
            transaction.IsDeleted = true;
            
            var changes = await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return changes > 0
                ? OperationResult.Success(Enum_StatusCode.OK, "Транзакция успешно удалена")
                : OperationResult.Failure(Enum_StatusCode.ServerError, "Изменения не сохранены");
        }
        catch (Exception ex)
        {
            return OperationResult.FromException(ex);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) 
        => _transactionRepository.ExistsAsync(id, cancellationToken);
}