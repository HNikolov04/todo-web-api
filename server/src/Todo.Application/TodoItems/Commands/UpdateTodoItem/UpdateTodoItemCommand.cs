using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Enums;

namespace Todo.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(Guid TodoItemId, string? NewTitle, DateTime? NewDueDate, bool? IsStarred, CompletionStatus? NewCompletionStatus) : ICommand;