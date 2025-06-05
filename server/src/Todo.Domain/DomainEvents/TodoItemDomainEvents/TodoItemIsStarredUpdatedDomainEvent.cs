namespace Todo.Domain.DomainEvents.TodoItemDomainEvents;

public sealed record TodoItemIsStarredUpdatedDomainEvent(Guid Id, Guid TodoItemId, bool IsStarred) : DomainEvent(Id);