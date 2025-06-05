using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.TodoItems.Queries.GetAllTodoItems;

public record GetAllTodoItemsQuery(Guid UserId, int Page = 1, int PageSize = 10, string? Filter = null, string? SortBy = null, bool Ascending = true)
    : IQuery<List<GetAllTodoItemsResponse>>;