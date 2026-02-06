namespace BankRUs.Api.Dtos.Transactions;

public record TransactionItemDto(
    Guid TransactionId,
    string Type,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);
