namespace BankRUs.Application.Common.Paging;

public record PagingDto(int Page, int PageSize, int TotalCount, int TotalPages)
{
    public static PagingDto From(int page, int pageSize, int totalCount)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return new PagingDto(page, pageSize, totalCount, totalPages);
    }
}