using BankRUs.Application.Common.Paging;


namespace BankRUs.Application.Common.Paging;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    PagingDto Paging
);

