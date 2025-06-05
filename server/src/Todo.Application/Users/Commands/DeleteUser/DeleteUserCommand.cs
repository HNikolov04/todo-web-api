using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : ICommand;