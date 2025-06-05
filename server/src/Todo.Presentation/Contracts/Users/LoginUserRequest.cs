namespace Todo.Presentation.Contracts.Users;

public sealed record LoginUserRequest(string Email, string Password);