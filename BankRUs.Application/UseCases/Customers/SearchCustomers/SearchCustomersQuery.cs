namespace BankRUs.Application.UseCases.Customers.SearchCustomers;

public record SearchCustomersQuery(int Page, int PageSize, string? Ssn);
