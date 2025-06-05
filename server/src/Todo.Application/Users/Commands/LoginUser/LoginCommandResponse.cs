namespace Todo.Application.Users.Commands.LoginUser;

public sealed record LoginUserResponse(string Token, string Username, string Email);