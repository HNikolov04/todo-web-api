using Microsoft.AspNetCore.Identity;
using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.User;

namespace Todo.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IDomainEventHolder
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Address Address { get; private set; }
    public bool IsDeleted { get; private set; }

    private User() { }

    private User(string username, string email, FirstName firstName, LastName lastName, Address address, string phoneNumber)
    {
        Id = Guid.NewGuid();
        UserName = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        PhoneNumber = phoneNumber;
    }

    public static Result<User> Create(string username, string email, FirstName firstName, LastName lastName, Address address, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Failure<User>(DomainErrors.User.UsernameRequired);
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<User>(DomainErrors.User.EmailRequired);
        }

        return Result.Success(new User(username, email, firstName, lastName, address, phoneNumber));
    }

    public void UpdateFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }

    public void UpdateLastName(LastName lastName)
    {
        LastName = lastName;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }

    public void UpdateUsername(string username)
    {
        UserName = username;
    }

    public void UpdatePassword(string username)
    {
        UserName = username;
    }

    public void UpdatePhone(string username)
    {
        UserName = username;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}