using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.Notes.Queries.GetAllNotes;

public sealed record GetAllNotesQuery(Guid TodoItemId)
    : IQuery<List<GetAllNotesResponse>>;