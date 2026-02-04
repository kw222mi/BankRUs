using System.ComponentModel.DataAnnotations;

namespace BankRUs.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Koppling till bankkontot
    public Guid BankAccountId { get; set; }
    public BankAccount? BankAccount { get; set; }

    public TransactionType Type { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Amount { get; set; }   

    [MaxLength(140)]
    public string? Reference { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Viktigt för historiken: vad var saldot efter transaktionen?
    public decimal BalanceAfter { get; set; }
}
