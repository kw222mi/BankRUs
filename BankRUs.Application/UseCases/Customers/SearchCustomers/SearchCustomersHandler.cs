using BankRUs.Application.Common.Paging;
using BankRUs.Application.Repositories;
using BankRUs.Application.UseCases.Customers.ListCustomers;

namespace BankRUs.Application.UseCases.Customers.SearchCustomers;

public class SearchCustomersHandler
{
    private readonly ICustomerRepository _repo;

    public SearchCustomersHandler(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<CustomerListItem>> HandleAsync(SearchCustomersQuery query, int maxPageSize)
    {
        var page = query.Page < 1 ? 1 : query.Page;

        var pageSize = query.PageSize < 1 ? 20 : query.PageSize;
        if (pageSize > maxPageSize) pageSize = maxPageSize;

        var (items, totalItems) = await _repo.ListAsync(page, pageSize, query.Ssn);

        var paging = PagingDto.From(page, pageSize, totalItems);
        return new PagedResult<CustomerListItem>(items, paging);
    }
}
