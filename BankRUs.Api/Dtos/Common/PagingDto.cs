namespace BankRUs.Api.Dtos.Common;

public record PagingDto(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);
