using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.Customers.GetCustomer;
using BankRUs.Application.UseCases.Customers.ListCustomers;
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
        // AspNetUsers ligger i din Identity DbContext
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
      
        return await Task.FromResult<CustomerDetailsDto?>(null);
    }
}