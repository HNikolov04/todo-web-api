using Todo.Domain.Enums;

namespace Todo.Presentation.Contracts.TodoItems;

public sealed record UpdateTodoItemRequest(string Title, DateTime DueDate, bool IsStarred, CompletionStatus CompletionStatus);