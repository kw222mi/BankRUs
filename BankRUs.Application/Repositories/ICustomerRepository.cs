using BankRUs.Application.UseCases.Customers.GetCustomer;
using BankRUs.Application.UseCases.Customers.ListCustomers;

namespace BankRUs.Application.Repositories;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<CustomerListItem> Items, int TotalItems)> ListAsync(int page, int pageSize);

    Task<CustomerDetailsDto?> GetByIdAsync(string customerId);
}