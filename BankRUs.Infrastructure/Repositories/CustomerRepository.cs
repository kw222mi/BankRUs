using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.Customers.GetCustomer;
using BankRUs.Application.UseCases.Customers.ListCustomers;
using BankRUs.Domain.Entities;
using BankRUs.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _db;

    public CustomerRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<(IReadOnlyList<CustomerListItem> Items, int TotalItems)> ListAsync(int page, int pageSize)
    {
        var query = _db.Users
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new CustomerListItem(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email!
            ))
            .ToListAsync();

        return (items, total);
    }

    public async Task<CustomerDetailsDto?> GetByIdAsync(string customerId)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == customerId);

        if (user is null)
            return null;

        // Om du har _db.BankAccounts, använd den. Annars Set<BankAccount>()
        var bankAccountsQuery = _db.Set<BankAccount>()
            .AsNoTracking()
            .Where(a => a.UserId == user.Id)
            .OrderBy(a => a.AccountNumber);

        var bankAccounts = await bankAccountsQuery
            .Select(a => new CustomerBankAccountDto(
                a.Id,
                a.AccountNumber,
                a.Name,
                a.Balance,
                a.IsLocked
            ))
            .ToListAsync();

        return new CustomerDetailsDto(
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email!,
            BankAccounts: bankAccounts
        );
    }
}