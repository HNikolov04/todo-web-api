namespace Todo.Domain.DomainEvents.NoteDomainEvents;

public sealed record NoteTextUpdatedDomainEvent(Guid Id, Guid TodoItemId, string OldNoteText, string NewNoteText, DateTime CreatedOnUtc) : DomainEvent(Id);