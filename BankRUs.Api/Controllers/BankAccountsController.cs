using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Application.UseCases.CreateDeposit;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController : ControllerBase
{
    private readonly OpenBankAccountHandler _openBankAccountHandler;
    private readonly CreateDepositHandler _createDepositHandler;

    public BankAccountsController(OpenBankAccountHandler openBankAccountHandler, CreateDepositHandler createDepositHandler)
    {
        _openBankAccountHandler = openBankAccountHandler;
        _createDepositHandler = createDepositHandler;
    }

    // POST /api/bank-accounts
    // {
    //    "userId": "",
    //    "bankAccountName": "Semester"
    // }
    [HttpPost]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequestDto request)
    {
        var openBankAccountResult = await _openBankAccountHandler.HandleAsync(
            new OpenBankAccountCommand(UserId: request.UserId));

        // TODO: Hårdkodad information nedan ska komma från 
        // resultatobjektet
        var response = new BankAccountDto(
            Id: openBankAccountResult.Id,
            AccountNumber: "100.200.300",
            Name: "Standardkonto",
            IsLocked: false,
            Balance: 0m,
            UserId: Guid.NewGuid());

        return Created(string.Empty, response);
    }

    [Authorize]
    [HttpPost("{bankAccountId}/deposits")]
    public async Task<IActionResult> CreateDeposit([FromRoute] Guid bankAccountId,
    [FromBody] DepositRequestDto request)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();


        var createDepositResult = await _createDepositHandler.HandleAsync(
            new CreateDepositCommand(bankAccountId,
        request.Amount,
        request.Reference,
        userId));

        if (createDepositResult == null) {
            return NotFound();
        }

        var response = new DepositResponseDto(
     TransactionId: createDepositResult.TransactionId,
     Type: createDepositResult.Type,
     Amount: createDepositResult.Amount,
     Reference: createDepositResult.Reference,
     CreatedAt: createDepositResult.CreatedAt,
     BalanceAfter: createDepositResult.BalanceAfter
 );

        return Created(string.Empty, response);


    }
}
