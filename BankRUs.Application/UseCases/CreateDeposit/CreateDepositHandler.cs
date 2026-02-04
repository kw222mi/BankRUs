using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;


namespace BankRUs.Application.UseCases.CreateDeposit;

using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class CreateDepositHandler
    {

    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateDepositHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }


    public async Task<CreateDepositResult?> HandleAsync(CreateDepositCommand command)
    {
        // 1) Hämta konto (om saknas -> null => controller returnerar 404)
        var account = await _bankAccountRepository.GetByIdAsync(command.BankAccountId);
        if (account is null)
            return null;

        // 2) Affärsvalidering
        if (command.Amount <= 0)
            throw new ValidationException("Amount must be greater than 0.");

        if (HasMoreThanTwoDecimals(command.Amount))
            throw new ValidationException("Amount can have at most 2 decimals.");

        if (command.Reference is not null && command.Reference.Length > 140)
            throw new ValidationException("Reference can be at most 140 characters.");

        // 3) Uppdatera saldo
        account.Deposit(command.Amount, command.Reference);
        var newBalance = account.Balance;


        // 4) Skapa transaktion
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = account.Id,
            Type = TransactionType.Deposit,                       
            Amount = command.Amount,
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            BalanceAfter = newBalance
        };

        // 5) Spara till DB (atomiskt)
        await _bankAccountRepository.AddTransactionAsync(transaction);
        await _bankAccountRepository.SaveChangesAsync();

        // 6) Returnera resultat (Application-result, inte API-DTO)
        return new CreateDepositResult(
            TransactionId: transaction.Id,
            Type: transaction.Type,
            Amount: transaction.Amount,
            Reference: transaction.Reference,
            CreatedAt: transaction.CreatedAt,
            BalanceAfter: transaction.BalanceAfter
        );
    }

    private static bool HasMoreThanTwoDecimals(decimal amount)
    {
        // Ex: 10.123 -> true, 10.12 -> false
        return decimal.Round(amount, 2) != amount;
    }
}

