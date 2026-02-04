
using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            SocialSecurityNumber = request.SocialSecurityNumber.Trim(),
            Email = request.Email.Trim()
        };

        string password = "Secret#1";

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception("Unable to create user");
        }

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        return new CreateUserResult(UserId: Guid.Parse(user.Id));
    }
}
