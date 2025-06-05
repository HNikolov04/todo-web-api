using Microsoft.AspNetCore.Identity;
using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.User;

namespace Todo.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var createObjectsResult = CreateAndValidateValueObjects(request.FirstName, request.LastName, request.Address);

        if (createObjectsResult.IsFailure)
        {
            return Result.Failure(createObjectsResult.Errors);
        }

        var userResult = User.Create(
            request.Username,
            request.Email,
            createObjectsResult.Value.FirstName,
            createObjectsResult.Value.LastName,
            createObjectsResult.Value.Address,
            request.PhoneNumber
        );

        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Errors);
        }

        var identityResult = await _userManager.CreateAsync(userResult.Value, request.Password);

        if (!identityResult.Succeeded)
        {
            var identityErrors = identityResult.Errors
                .Select(e => new Error(e.Code, e.Description))
                .ToArray();

            return Result.Failure(identityErrors);
        }

        return Result.Success();
    }

    private static Result<(FirstName FirstName, LastName LastName, Address Address)> CreateAndValidateValueObjects(
        string firstName, string lastName, string address)
    {
        var firstNameResult = FirstName.Create(firstName);
        var lastNameResult = LastName.Create(lastName);
        var addressResult = Address.Create(address);

        if (firstNameResult.IsFailure || lastNameResult.IsFailure || addressResult.IsFailure)
        {
            var errors = firstNameResult.Errors
                .Concat(lastNameResult.Errors)
                .Concat(addressResult.Errors)
                .ToArray();

            return Result.Failure<(FirstName, LastName, Address)>(errors);
        }

        return Result.Success((
            firstNameResult.Value,
            lastNameResult.Value,
            addressResult.Value
        ));
    }
}