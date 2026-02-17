using BankRUs.Api;
using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.Customers.GetCustomer;
using BankRUs.Application.UseCases.Customers.ListCustomers;
using BankRUs.Infrastructure.Identity; // <- Roles
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankRUs.Application.UseCases.Customers.GetCustomer;

[ApiController]
[Route("api/customers")]
[Authorize(Roles = Roles.CustomerService)]
public class CustomersController : ControllerBase
{
    private readonly ListCustomersHandler _handler;
    private readonly QueryParamsOptions _opts;
    private readonly GetCustomerHandler _getCustomerHandler;

    public CustomersController(ListCustomersHandler handler, QueryParamsOptions opts, GetCustomerHandler getCustomerHandler)
    {
        _handler = handler;
        _opts = opts;
        _getCustomerHandler = getCustomerHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _handler.HandleAsync(
            new ListCustomersQuery(page, pageSize),
            _opts.MaxPageSize);

        return Ok(new
        {
            data = result.Items,
            page = result.Paging.Page,
            pageSize = result.Paging.PageSize,
            totalItems = result.Paging.TotalCount,
            totalPages = result.Paging.TotalPages
        });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _getCustomerHandler.HandleAsync(new GetCustomerQuery(id));

        if (result is null)
            return NotFound();

        return Ok(new
        {
            id = result.Id,
            firstName = result.FirstName,
            lastName = result.LastName,
            email = result.Email,
            bankAccounts = result.BankAccounts.Select(a => new
            {
                id = a.Id,
                bankAccountNumber = a.AccountNumber,
                name = a.Name,
                balance = a.Balance,
                isLocked = a.IsLocked
            })
        });
    }
}