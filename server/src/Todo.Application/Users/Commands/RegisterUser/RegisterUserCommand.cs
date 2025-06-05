using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Address
) : ICommand;