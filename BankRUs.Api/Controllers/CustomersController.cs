using BankRUs.Api;
using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.Customers.ListCustomers;
using BankRUs.Infrastructure.Identity; // <- Roles
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customers")]
[Authorize(Roles = Roles.CustomerService)]
public class CustomersController : ControllerBase
{
    private readonly ListCustomersHandler _handler;
    private readonly QueryParamsOptions _opts;

    public CustomersController(ListCustomersHandler handler, QueryParamsOptions opts)
    {
        _handler = handler;
        _opts = opts;
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
}