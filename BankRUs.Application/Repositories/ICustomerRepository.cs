using BankRUs.Application.UseCases.Customers.ListCustomers;

namespace BankRUs.Application.Repositories;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<CustomerListItem> Items, int TotalItems)> ListAsync(int page, int pageSize);

    // TODO: US10 uppgift 2
    //Task<CustomerDetailsDto?> GetByIdAsync(string customerId);
}