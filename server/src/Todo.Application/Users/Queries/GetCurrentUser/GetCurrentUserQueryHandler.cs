using Microsoft.AspNetCore.Identity;
using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Errors;
using Todo.Domain.Shared;

namespace Todo.Application.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly UserManager<User> _userManager;

    public GetCurrentUserQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            return Result.Failure<GetCurrentUserResponse>(ApplicationErrors.User.NotFound);
        }

        var response = new GetCurrentUserResponse(
            user.UserName,
            user.Email,
            user.FirstName.Value,
            user.LastName.Value,
            user.PhoneNumber,
            user.Address.Value);

        return Result.Success(response);
    }
}