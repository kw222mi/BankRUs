namespace BankRUs.Application.UseCases.ListTransactions;

public record ListTransactionItem(
    Guid TransactionId,
    string Type,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);
