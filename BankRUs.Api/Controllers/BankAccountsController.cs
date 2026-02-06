using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Application.Common.Exceptions;

using BankRUs.Application.UseCases.CreateDeposit;
using BankRUs.Application.UseCases.CreateWithdrawal;
using BankRUs.Application.UseCases.ListTransactions;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

using System.Security.Claims;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController : ControllerBase
{
    private readonly OpenBankAccountHandler _openBankAccountHandler;
    private readonly CreateDepositHandler _createDepositHandler;
    private readonly CreateWithdrawalHandler _createWithdrawalHandler;
    private readonly ListTransactionsHandler _listTransactionsHandler;

    public BankAccountsController(OpenBankAccountHandler openBankAccountHandler,
        CreateDepositHandler createDepositHandler,
        CreateWithdrawalHandler createWithdrawalHandler,
        ListTransactionsHandler listTransactionsHandler)
    {
        _openBankAccountHandler = openBankAccountHandler;
        _createDepositHandler = createDepositHandler;
        _createWithdrawalHandler = createWithdrawalHandler;
        _listTransactionsHandler = listTransactionsHandler;
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

        if (createDepositResult == null)
        {
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
    [Authorize]
    [HttpPost("{bankAccountId}/withdrawals")]


    public async Task<IActionResult> CreateWithdrawal(
    [FromRoute] Guid bankAccountId,
    [FromBody] WithdrawRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var result = await _createWithdrawalHandler.HandleAsync(
                new CreateWithdrawalCommand(
                    bankAccountId,
                    request.Amount,
                    request.Reference,
                    userId!)
            );

            if (result is null)
                return NotFound();

            var response = new WithdrawResponseDto(
                TransactionId: result.TransactionId,
                Type: result.Type,
                Amount: result.Amount,
                Reference: result.Reference,
                CreatedAt: result.CreatedAt,
                BalanceAfter: result.BalanceAfter
            );

            return Created(string.Empty, response);
        }
        catch (InsufficientFundsException ex)
        {
            return Conflict(new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Insufficient funds",
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message
            });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            });
        }
    }

    [ApiController]
    [Route("api/me")]
    [Authorize]
    public class MeController : ControllerBase
    {
        private readonly ListTransactionsHandler _handler;

        public MeController(ListTransactionsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("accounts/{bankAccountId:guid}/transactions")]
        public async Task<IActionResult> ListTransactions(
            [FromRoute] Guid bankAccountId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sort = "desc"
            )


        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            sort = sort?.ToLower() == "asc" ? "asc" : "desc";


            var result = await _handler.HandleAsync(new ListTransactionsQuery(
                BankAccountId: bankAccountId,
                UserId: userId,
                Page: page,
                PageSize: pageSize,
                Sort: sort
            ));

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
