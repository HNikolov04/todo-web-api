namespace Todo.Application.ChangeLogEntries.Queries.GetAllChangeLogEntries;

public sealed record GetAllChangeLogEntriesResponse(
    Guid Id,
    string Text,
    DateTime CreateOnUtc
);
