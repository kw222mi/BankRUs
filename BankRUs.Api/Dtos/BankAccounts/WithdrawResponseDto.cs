namespace BankRUs.Api.Dtos.BankAccounts;

public record WithdrawResponseDto(
    Guid TransactionId,
    string Type,
    decimal Amount,
   
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);
