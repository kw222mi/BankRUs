using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ApplicationDbContext _db;
    public BankAccountRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Add(BankAccount bankAccount)
    {
        _db.BankAccounts.Add(bankAccount);
        await _db.SaveChangesAsync();
    }

    public async Task<BankAccount?> GetByIdAsync(Guid bankAccountId)
    {
        // Hämta kontot. Inkludera inte Transactions om du inte behöver just nu.
        return await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == bankAccountId);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _db.Transactions.AddAsync(transaction);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public Task<int> CountTransactionsAsync(Guid bankAccountId)
        => _db.Transactions.CountAsync(t => t.BankAccountId == bankAccountId);

    public async Task<IReadOnlyList<Transaction>> ListTransactionsAsync(Guid bankAccountId, int page, int pageSize, string sort)
    {
        var skip = (page - 1) * pageSize;

        var query = _db.Transactions
         .Where(t => t.BankAccountId == bankAccountId);

        query = sort == "asc"
            ? query.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id)
            : query.OrderByDescending(t => t.CreatedAt).ThenByDescending(t => t.Id);

        return await query
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

   
}
