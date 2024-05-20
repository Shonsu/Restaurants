using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.Shared;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(
    ILogger<UnassignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
) : UserRoleCommandHandlerBase<UnassignUserRoleCommand>(userManager, roleManager)
{
    protected override async Task ChangeUserRoleAsync(User user, IdentityRole role)
    {
        logger.LogWarning("Unassign role {RoleName} from user {UserEmail}", role.Name, user.Email);
        await userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}
