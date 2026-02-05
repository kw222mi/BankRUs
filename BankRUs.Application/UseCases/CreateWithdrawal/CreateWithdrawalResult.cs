namespace BankRUs.Application.UseCases.CreateWithdrawal;

public record CreateWithdrawalResult(
    Guid TransactionId,
    string Type,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);
