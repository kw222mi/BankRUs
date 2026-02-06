namespace BankRUs.Application.UseCases.ListTransactions;

public record ListTransactionsResult(
    Guid AccountId,
  
    decimal Balance,
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyList<ListTransactionItem> Items
);
