using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Notes.Commands.CreateNote;

public sealed record CreateNoteCommand(Guid TodoItemId, string Text) : ICommand<Guid>;