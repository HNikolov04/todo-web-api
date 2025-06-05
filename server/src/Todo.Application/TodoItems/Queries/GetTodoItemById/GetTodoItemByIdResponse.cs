namespace Todo.Application.TodoItems.Queries.GetTodoItemById;

public sealed record GetTodoItemByIdResponse(Guid Id, string TodoTitle, DateTime DueDate, bool IsStarred, string CompletionStatus);