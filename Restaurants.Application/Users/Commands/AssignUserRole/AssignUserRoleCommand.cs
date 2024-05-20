using Restaurants.Application.Users.Commands.Shared;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public record AssignUserRoleCommand : UserRoleCommand
{
    public AssignUserRoleCommand(string UserEmail, string RoleName)
        : base(UserEmail, RoleName) { }
}
