namespace Todo.Application.Notes.Queries.GetNoteById;

public sealed record GetNoteByIdResponse(Guid Id, string Text);