using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public record UnassignUserRoleCommand(string UserEmail, string RoleName) : IRequest { }
