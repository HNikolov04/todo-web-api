using Microsoft.AspNetCore.Identity;
using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Errors;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.User;

namespace Todo.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            return Result.Failure(ApplicationErrors.User.NotFound);
        }

        var updateResult = ApplyUpdates(user, request);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Errors);
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result.Failure(ApplicationErrors.User.UpdateFailed);
        }

        return Result.Success();
    }

    private static Result ApplyUpdates(User user, UpdateUserCommand request)
    {
        if (request.UserName is not null)
        {
            //if?

            user.UpdateUsername(request.UserName);
        }

        if (request.FirstName is not null)
        {
            var firstNameResult = FirstName.Create(request.FirstName);

            if (firstNameResult.IsFailure)
            {
                return Result.Failure(firstNameResult.Errors);
            }

            user.UpdateFirstName(firstNameResult.Value);
        }

        if (request.LastName is not null)
        {
            var lastNameResult = LastName.Create(request.LastName);

            if (lastNameResult.IsFailure)
            {
                return Result.Failure(lastNameResult.Errors);
            }

            user.UpdateLastName(lastNameResult.Value);
        }

        if (request.Address is not null)
        {
            var addressResult = Address.Create(request.Address);

            if (addressResult.IsFailure)
            {
                return Result.Failure(addressResult.Errors);
            }

            user.UpdateAddress(addressResult.Value);
        }

        return Result.Success();
    }
}