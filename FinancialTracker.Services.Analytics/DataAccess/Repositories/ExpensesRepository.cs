using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class ExpensesRepository(AppDbContext db) : IExpensesRepository
{
    public async Task<List<Expense>> GetExpensesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.UserId == userId &&
                       x.ExpenseTime >= startDate.ToUniversalTime()
                        && x.ExpenseTime <= endDate.ToUniversalTime())
            .ToListAsync(cancellationToken);
    }
    
    public async Task<decimal> GetSumOfExpensesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken)
    {
        var query = db.Expenses;
        return await query
            .Where(x => x.UserId == userId &&
                        x.ExpenseTime <= upToDate.ToUniversalTime())
            .SumAsync(e => e.Amount, cancellationToken);
    }

    public async Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request, CancellationToken cancellationToken)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.UserId == request.UserId &&
                       x.AccountId == request.AccountId &&
                       x.ExpenseTime >= request.StartDate.ToUniversalTime() &&
                       x.ExpenseTime <= request.EndDate.ToUniversalTime())
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddAsync(Expense expense, CancellationToken cancellationToken)
    {
        db.Expenses.Add(expense);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<CategoryAggregate>> GetExpenseTotalByCategoryAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var startUtc = startDate.Kind == DateTimeKind.Unspecified
        ? DateTime.SpecifyKind(startDate, DateTimeKind.Utc)
        : startDate.ToUniversalTime();

        var endUtc = endDate.Kind == DateTimeKind.Unspecified
        ? DateTime.SpecifyKind(endDate, DateTimeKind.Utc)
        : endDate.ToUniversalTime();

        return await db.Expenses
        .AsNoTracking()
        .Where(e => e.UserId == userId &&
                    e.ExpenseTime >= startUtc &&
                    e.ExpenseTime <= endUtc)
        .GroupBy(e => e.Category)
        .Select(g => new CategoryAggregate(
            g.Key ?? "Без категории",
            g.Sum(x => x.Amount),
            g.Count()))
        .ToListAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid expenceId, CancellationToken cancellationToken)
    {
        await db.Expenses
            .Where(i => i.ExpenseId == expenceId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Guid expenseId,
        string? category,
        string currency,
        decimal amount,
        DateTime expenseTime,
        CancellationToken cancellationToken)
    {
        await db.Expenses
        .Where(e => e.ExpenseId == expenseId)
        .ExecuteUpdateAsync(
            e => e
                .SetProperty(p => p.Category, category)
                .SetProperty(p => p.Currency, currency)
                .SetProperty(p => p.Amount, amount)
                .SetProperty(p => p.ExpenseTime, expenseTime),
            cancellationToken);
    }


    public async Task<IDbContextTransaction> CreateTransactionAsync(CancellationToken ct)
        => await db.Database.BeginTransactionAsync(ct);
}