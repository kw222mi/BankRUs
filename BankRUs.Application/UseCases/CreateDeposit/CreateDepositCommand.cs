using BankRUs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.CreateDeposit;

    public record CreateDepositCommand(
       Guid BankAccountId,
        decimal Amount,
        string Reference,
        string UserId 
        );
  

