using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;

namespace Todo.Application.TodoItems.Queries.GetTodoItemById;

public sealed class GetTodoItemByIdQueryHandler : IQueryHandler<GetTodoItemByIdQuery, GetTodoItemByIdResponse>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetTodoItemByIdQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<GetTodoItemByIdResponse>> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure<GetTodoItemByIdResponse>(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var response = new GetTodoItemByIdResponse(
            todoItem.Id,
            todoItem.Title.Value,
            todoItem.DueDate.Value,
            todoItem.IsStarred,
            todoItem.CompletionStatus.ToString()
        );

        return Result.Success(response);
    }
}