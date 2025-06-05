using Todo.Domain.Primitives;

namespace Todo.Domain.DomainEvents;

public abstract record DomainEvent(Guid Id) : IDomainEvent;