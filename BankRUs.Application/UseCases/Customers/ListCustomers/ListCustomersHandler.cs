using BankRUs.Application.Common.Paging;
using BankRUs.Application.Repositories;

namespace BankRUs.Application.UseCases.Customers.ListCustomers;

public class ListCustomersHandler
{
    private readonly ICustomerRepository _repo;

    public ListCustomersHandler(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<CustomerListItem>> HandleAsync(ListCustomersQuery query, int maxPageSize)
    {
        var page = query.Page < 1 ? 1 : query.Page;

        var pageSize = query.PageSize < 1 ? 20 : query.PageSize;
        if (pageSize > maxPageSize) pageSize = maxPageSize;

        var (items, totalItems) = await _repo.ListAsync(page, pageSize);

        var paging = PagingDto.From(page, pageSize, totalItems);

        return new PagedResult<CustomerListItem>(
            Items: items,
            Paging: paging
        );
    }
}