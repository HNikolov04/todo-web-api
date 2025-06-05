using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResponse>;