using Microsoft.AspNetCore.Identity;
using Todo.Application.Abstractions.CQRS;
using Todo.Application.Abstractions.Services;
using Todo.Domain.Entities;
using Todo.Domain.Errors;
using Todo.Domain.Shared;

namespace Todo.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager ,IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Failure<LoginUserResponse>(ApplicationErrors.User.InvalidCredentials);
        }

        var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!isPasswordValid.Succeeded)
        {
            return Result.Failure<LoginUserResponse>(ApplicationErrors.User.InvalidCredentials);
        }

        var token = _jwtProvider.Generate(user);

        var response = new LoginUserResponse(token, user.UserName!, user.Email!);

        return Result.Success(response);
    }
}