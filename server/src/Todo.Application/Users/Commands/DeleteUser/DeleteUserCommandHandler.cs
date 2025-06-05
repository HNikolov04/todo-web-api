using Microsoft.AspNetCore.Identity;
using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Errors;
using Todo.Domain.Shared;

namespace Todo.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            return Result.Failure(ApplicationErrors.User.NotFound);
        }

        user.MarkAsDeleted();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result.Failure(ApplicationErrors.User.DeleteFailed);
        }

        return Result.Success();
    }
}