using MediatR;
using Restaurants.Application.Users.Commands.Shared;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public record UnassignUserRoleCommand : UserRoleCommand
{
    public UnassignUserRoleCommand(string UserEmail, string RoleName)
        : base(UserEmail, RoleName) { }
}
