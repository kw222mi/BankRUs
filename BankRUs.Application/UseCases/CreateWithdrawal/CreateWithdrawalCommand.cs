namespace BankRUs.Application.UseCases.CreateWithdrawal;

public record CreateWithdrawalCommand(
    Guid BankAccountId,
    decimal Amount,
    string? Reference,
    string UserId
);
