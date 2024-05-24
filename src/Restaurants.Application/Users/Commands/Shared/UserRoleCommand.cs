using MediatR;

namespace Restaurants.Application.Users.Commands.Shared;

public record UserRoleCommand(string UserEmail, string RoleName) : IRequest { }
