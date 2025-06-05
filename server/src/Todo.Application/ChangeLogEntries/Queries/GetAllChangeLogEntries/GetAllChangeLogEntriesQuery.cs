using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.ChangeLogEntries.Queries.GetAllChangeLogEntries;

public sealed record GetAllChangeLogEntriesQuery(Guid TodoItemId) : IQuery<List<GetAllChangeLogEntriesResponse>>;