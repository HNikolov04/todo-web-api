namespace Todo.Presentation.Contracts.Users;

public sealed record RegisterUserRequest(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Address
);