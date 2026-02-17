using BankRUs.Application.Common.Paging;

namespace BankRUs.Application.UseCases.ListTransactions;

public record ListTransactionsResult(
    Guid AccountId,
    decimal Balance,
   PagedResult<ListTransactionItem> Transactions
  
);
