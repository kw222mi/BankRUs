using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.CreateDeposit;

public record CreateDepositResult(
    Guid TransactionId,
    TransactionType Type,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);
