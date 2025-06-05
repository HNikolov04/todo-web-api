namespace Todo.Domain.DomainEvents.NoteDomainEvents;

public sealed record NoteCreatedDomainEvent(Guid Id, Guid TodoItemId, string NoteText, DateTime CreatedOnUtc) : DomainEvent(Id);