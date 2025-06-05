using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Notes.Queries.GetNoteById;

public sealed record GetNoteByIdQuery(Guid TodoItemId, Guid NoteId) : IQuery<GetNoteByIdResponse>;