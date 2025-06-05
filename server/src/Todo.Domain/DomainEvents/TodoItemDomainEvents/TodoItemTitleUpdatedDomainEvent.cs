namespace Todo.Domain.DomainEvents.TodoItemDomainEvents;

public sealed record TodoItemTitleUpdatedDomainEvent(Guid Id, Guid TodoItemId, string OldTitle) : DomainEvent(Id);