namespace Todo.Application.TodoItems.Queries.GetAllTodoItems;

public sealed record GetAllTodoItemsResponse(Guid Id, string TodoTitle, DateTime DueDate, bool IsStarred, string CompletionStatus);