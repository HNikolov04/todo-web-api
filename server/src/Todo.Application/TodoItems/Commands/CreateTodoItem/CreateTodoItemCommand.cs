using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.TodoItems.Commands.CreateTodoItem;

public sealed record CreateTodoItemCommand(Guid UserId, string Title, DateTime DueDateTime) : ICommand<Guid>;