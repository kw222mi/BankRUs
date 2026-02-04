using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface IBankAccountRepository
{
    Task Add(BankAccount bankAccount);
    Task AddTransactionAsync(Transaction transaction);
    Task SaveChangesAsync();
    Task<BankAccount?> GetByIdAsync(Guid bankAccountId);

}
