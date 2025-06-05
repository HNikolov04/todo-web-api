using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Abstractions.Services;
using Todo.Application.Users.Commands.DeleteUser;
using Todo.Application.Users.Commands.LoginUser;
using Todo.Application.Users.Commands.RegisterUser;
using Todo.Application.Users.Commands.UpdateUser;
using Todo.Application.Users.Queries.GetCurrentUser;
using Todo.Presentation.Abstractions;
using Todo.Presentation.Constants;
using Todo.Presentation.Contracts.Users;

namespace Todo.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Users.Base)]
public class UserController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public UserController(IMediator mediator, IUserContext userContext) : base(mediator)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost(ApiRoutes.Users.Register)]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Username,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Address
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }

    [HttpPost(ApiRoutes.Users.Login)]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet(ApiRoutes.Users.Me)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery(_userContext.UserId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPut(ApiRoutes.Users.Me)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(
            _userContext.UserId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request.Address
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpDelete(ApiRoutes.Users.Me)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(_userContext.UserId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}
