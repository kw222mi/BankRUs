using System.ComponentModel.DataAnnotations;

namespace BankRUs.Api.Dtos.BankAccounts;

    public record DepositRequestDto(

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        decimal Amount,

        [Required]
        [MaxLength(140)]
        string Reference
        );

