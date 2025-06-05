namespace Todo.Presentation.Contracts.Users;

public sealed record UpdateUserRequest(string UserName, string FirstName, string LastName, string Address);