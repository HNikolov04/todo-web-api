namespace Todo.Domain.DomainEvents.NoteDomainEvents;

public sealed record NoteRemovedDomainEvent(Guid Id, Guid TodoItemId, string NoteText, DateTime CreatedOnUtc) : DomainEvent(Id);