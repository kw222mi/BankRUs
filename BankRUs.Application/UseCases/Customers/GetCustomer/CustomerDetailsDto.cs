namespace BankRUs.Application.UseCases.Customers.GetCustomer;

public record CustomerDetailsDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyList<CustomerBankAccountDto> BankAccounts
);

public record CustomerBankAccountDto(
    Guid Id,
    string AccountNumber,
    string Name,
    decimal Balance
);