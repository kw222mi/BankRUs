using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Repositories;

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
}
