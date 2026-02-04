using BankRUs.Domain.Entities;

namespace BankRUs.Api.Dtos.BankAccounts;

    public record DepositResponseDto
  (
        Guid TransactionId,
        TransactionType Type,
        decimal Amount,
        string? Reference,
        DateTime CreatedAt,
        decimal BalanceAfter


);

