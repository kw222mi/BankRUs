using BankRUs.Application.Repositories;

namespace BankRUs.Application.UseCases.Customers.GetCustomer;

public class GetCustomerHandler
{
    private readonly ICustomerRepository _repo;

    public GetCustomerHandler(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public Task<CustomerDetailsDto?> HandleAsync(GetCustomerQuery query)
        => _repo.GetByIdAsync(query.CustomerId);
}