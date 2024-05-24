using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.Shared;

public abstract class UserRoleCommandHandlerBase<TCommand> : IRequestHandler<TCommand>
    where TCommand : UserRoleCommand
{
    protected readonly UserManager<User> userManager;
    protected readonly RoleManager<IdentityRole> roleManager;

    protected UserRoleCommandHandlerBase(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        User user = await FindUserAsync(request.UserEmail);
        IdentityRole role = await FindRoleAsync(request.RoleName);
        await ChangeUserRoleAsync(user, role);
    }

    private async Task<User> FindUserAsync(string userEmail)
    {
        return await userManager.FindByEmailAsync(userEmail)
            ?? throw new NotFoundException(nameof(User), userEmail);
    }

    private async Task<IdentityRole> FindRoleAsync(string roleName)
    {
        return await roleManager.FindByNameAsync(roleName)
            ?? throw new NotFoundException(nameof(User), roleName);
    }

    protected abstract Task ChangeUserRoleAsync(User user, IdentityRole role);
}
