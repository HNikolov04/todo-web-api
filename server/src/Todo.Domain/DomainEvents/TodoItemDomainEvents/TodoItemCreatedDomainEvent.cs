namespace Todo.Domain.DomainEvents.TodoItemDomainEvents;

public sealed record TodoItemCreatedDomainEvent(Guid Id, Guid TodoItemId) : DomainEvent(Id);
