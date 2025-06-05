using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string? UserName,
    string? FirstName,
    string? LastName,
    string? Address) : ICommand;