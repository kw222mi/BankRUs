using BankRUs.Application.Common.Exceptions;
using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankRUs.Application.UseCases.CreateWithdrawal;

public class CreateWithdrawalHandler
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateWithdrawalHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<CreateWithdrawalResult?> HandleAsync(CreateWithdrawalCommand command)
    {
        // 1) Hämta konto
        var account = await _bankAccountRepository.GetByIdAsync(command.BankAccountId);
        if (account is null)
            return null;

        // 2) Ägarskap (om du vill skydda)
        if (account.UserId != command.UserId)
            return null; // eller throw UnauthorizedAccessException

        // 3) Affärsvalidering
        if (command.Amount <= 0)
            throw new ValidationException("Amount must be greater than 0.");

        if (HasMoreThanTwoDecimals(command.Amount))
            throw new ValidationException("Amount can have at most 2 decimals.");

        if (command.Reference is not null && command.Reference.Length > 140)
            throw new ValidationException("Reference can be at most 140 characters.");

        if (account.Balance < command.Amount)
            throw new InsufficientFundsException(
                $"Account balance is {account.Balance:0.00} SEK but withdrawal amount is {command.Amount:0.00} SEK.");


        // 4) Uppdatera saldo via domänmetod
        account.Withdraw(command.Amount, command.Reference);

        // 5) Skapa transaktion
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = account.Id,
            Type = TransactionType.Withdrawal,
            Amount = command.Amount,
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            BalanceAfter = account.Balance
        };

        await _bankAccountRepository.AddTransactionAsync(transaction);
        await _bankAccountRepository.SaveChangesAsync();

        return new CreateWithdrawalResult(
            TransactionId: transaction.Id,
            Type: transaction.Type.ToString(),
            Amount: transaction.Amount,
           
            Reference: transaction.Reference,
            CreatedAt: transaction.CreatedAt,
            BalanceAfter: transaction.BalanceAfter
        );
    }

    private static bool HasMoreThanTwoDecimals(decimal amount)
        => decimal.Round(amount, 2) != amount;
}
