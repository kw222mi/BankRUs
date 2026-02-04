
using System.ComponentModel.DataAnnotations;

namespace BankRUs.Domain.Entities;

public class BankAccount
{
    public BankAccount(string accountNumber, string name, string userId)
    {
        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        Name = name;
        UserId = userId;
    }

    public Guid Id { get; protected set; }

    [MaxLength(25)]
    public string AccountNumber { get; protected set; }
    
    [MaxLength(25)]
    public string Name { get; protected set; }
    
    public bool IsLocked { get; protected set; }

    public decimal Balance { get; protected set; }
    public string UserId { get; protected set; }

   
    public void Withdraw(decimal amount, string? reference) { }

    public void Deposit(decimal amount, string? reference)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Amount must be greater than zero.");

        Balance += amount;
    }

}

// Konstruktor
// Object initializer-syntax


