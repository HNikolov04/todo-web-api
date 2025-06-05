namespace Todo.Domain.DomainEvents.TodoItemDomainEvents;

public sealed record TodoItemDueDateUpdatedDomainEvent(Guid Id, Guid TodoItemId, DateTime OldDateTime, DateTime CreatedOnUtc) : DomainEvent(Id);