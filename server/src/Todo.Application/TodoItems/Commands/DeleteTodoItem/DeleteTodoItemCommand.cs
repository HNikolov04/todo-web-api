using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand(Guid TodoItemId) : ICommand;