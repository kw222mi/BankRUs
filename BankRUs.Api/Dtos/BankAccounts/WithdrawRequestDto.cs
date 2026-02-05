using System.ComponentModel.DataAnnotations;

namespace BankRUs.Api.Dtos.BankAccounts;

public class WithdrawRequestDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; init; }

    [MaxLength(140)]
    public string? Reference { get; init; }
}
