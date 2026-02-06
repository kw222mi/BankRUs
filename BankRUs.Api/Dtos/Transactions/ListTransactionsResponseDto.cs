using BankRUs.Api.Dtos.Common;

namespace BankRUs.Api.Dtos.Transactions;

public record ListTransactionsResponseDto(
    Guid AccountId,
  
    decimal Balance,
    PagingDto Paging,
    IReadOnlyList<TransactionItemDto> Items
);
