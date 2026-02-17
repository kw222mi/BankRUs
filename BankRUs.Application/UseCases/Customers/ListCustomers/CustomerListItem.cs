namespace BankRUs.Application.UseCases.Customers.ListCustomers;

public record CustomerListItem(
    string Id,
    string FirstName,
    string LastName,
    string Email
);
