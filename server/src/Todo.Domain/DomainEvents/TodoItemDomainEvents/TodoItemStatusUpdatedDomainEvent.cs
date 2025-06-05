namespace Todo.Domain.DomainEvents.TodoItemDomainEvents;

public sealed record TodoItemStatusUpdatedDomainEvent(Guid Id, Guid TodoItemId, string OldStatus) : DomainEvent(Id);