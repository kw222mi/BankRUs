namespace BankRUs.Application.Authentication.AuthenticateUser;

public record AuthenticatedUser(
    string UserId,
    string UserName,
    string Email,
    IEnumerable<string> Roles
)
{
    public AuthenticatedUser(string userId, string userName, string email)
        : this(userId, userName, email, Array.Empty<string>())
    { }
}