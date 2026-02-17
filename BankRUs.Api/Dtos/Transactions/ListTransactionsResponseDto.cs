using BankRUs.Application.Common.Paging;

namespace BankRUs.Api.Dtos.Transactions;

public record ListTransactionsResponseDto(
    Guid AccountId,
  
    decimal Balance,
    PagingDto Paging,
    IReadOnlyList<TransactionItemDto> Items
);
