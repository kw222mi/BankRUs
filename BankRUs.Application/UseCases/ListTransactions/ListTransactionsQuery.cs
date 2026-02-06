namespace BankRUs.Application.UseCases.ListTransactions;

public record ListTransactionsQuery(
    Guid BankAccountId,
    string UserId,
    int Page,
    int PageSize,
    string Sort

);
