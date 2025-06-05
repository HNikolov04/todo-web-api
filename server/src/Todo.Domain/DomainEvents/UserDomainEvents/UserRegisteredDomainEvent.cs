namespace Todo.Domain.DomainEvents.UserDomainEvents;

public sealed record UserRegisteredDomainEvent(Guid Id) : DomainEvent(Id);