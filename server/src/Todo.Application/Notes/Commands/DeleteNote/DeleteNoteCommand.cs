using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Notes.Commands.DeleteNote;

public sealed record DeleteNoteCommand(Guid TodoItemId, Guid NoteId) : ICommand;