using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.Shared;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(
    ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
) : UserRoleCommandHandlerBase<AssignUserRoleCommand>(userManager, roleManager)
{
    protected override async Task ChangeUserRoleAsync(User user, IdentityRole role)
    {
        logger.LogInformation("Assign {UserRole} role to {UserEmail}", role.Name, user.Email);
        await userManager.AddToRoleAsync(user, role.Name!);
    }
}
