namespace Todo.Application.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserResponse(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Address);