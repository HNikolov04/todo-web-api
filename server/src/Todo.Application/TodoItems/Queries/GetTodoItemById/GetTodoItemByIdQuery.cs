using Todo.Application.Abstractions.CQRS;

namespace Todo.Application.TodoItems.Queries.GetTodoItemById;

public sealed record GetTodoItemByIdQuery(Guid TodoItemId)
    : IQuery<GetTodoItemByIdResponse>;