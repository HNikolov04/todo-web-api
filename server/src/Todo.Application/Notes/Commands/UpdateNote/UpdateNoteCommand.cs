using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Notes.Commands.UpdateNote;

public sealed record UpdateNoteCommand(Guid TodoItemId, Guid NoteId, string Text) : ICommand;