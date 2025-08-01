using Microsoft.EntityFrameworkCore.Storage;
using Wallet.Application.Interfaces.Repositories;

namespace Wallet.Infrastructure.Data.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly WalletPostgresDbContext _context;
    
    public UnitOfWork(
        WalletPostgresDbContext context,
        IAccountRepository accountRepository,
        ITagRepository tagRepository,
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository)
    {
        _context = context;
        AccountRepository = accountRepository;
        TagRepository = tagRepository;
        TransactionRepository = transactionRepository;
        CategoryRepository = categoryRepository;
    }

    public IAccountRepository AccountRepository { get; }
    public ITagRepository TagRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public ICategoryRepository CategoryRepository { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => _context.SaveChangesAsync(cancellationToken);
    
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _context.Database.BeginTransactionAsync(cancellationToken);
}