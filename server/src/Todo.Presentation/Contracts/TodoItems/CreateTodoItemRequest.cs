namespace Todo.Presentation.Contracts.TodoItems;

public sealed record CreateTodoItemRequest(string Title, DateTime DueDate);