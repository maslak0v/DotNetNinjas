using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private WalletPostgresDbContext _context;
    
    private IAccountRepository _accountRepository;
    private ITagRepository _tagRepository;
    private ITransactionTagRepository _transactionTagRepository;
    private ITransactionRepository _transactionRepository;

    public IAccountRepository AccountRepository => _accountRepository;
    public ITagRepository TagRepository => _tagRepository;
    public ITransactionRepository TransactionRepository => _transactionRepository;
    public ITransactionTagRepository TransactionTagRepository => _transactionTagRepository;
    
    
    public UnitOfWork(WalletPostgresDbContext context)
    {
        _context = context;
        _accountRepository = new AccountRepository(context);
        _tagRepository = new TagRepository(context);
        _transactionTagRepository = new TransactionTagRepository(context);
        _transactionRepository = new TransactionRepository(context);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
}
