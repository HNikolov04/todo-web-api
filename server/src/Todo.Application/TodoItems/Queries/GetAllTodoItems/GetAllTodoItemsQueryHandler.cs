using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;

namespace Todo.Application.TodoItems.Queries.GetAllTodoItems;

public sealed class GetAllTodoItemsQueryHandler : IQueryHandler<GetAllTodoItemsQuery, List<GetAllTodoItemsResponse>>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetAllTodoItemsQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<List<GetAllTodoItemsResponse>>> Handle(GetAllTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = await _todoItemRepository.GetAllAsync(
            request.UserId,
            request.Page,
            request.PageSize,
            request.Filter,
            request.SortBy,
            request.Ascending,
            cancellationToken
        );
        
        if (!todoItems.Any())
        {
            return Result.Failure<List<GetAllTodoItemsResponse>>(ApplicationErrors.Todo.NoItemsFound);
        }

        var response = todoItems
            .Select(todo => new GetAllTodoItemsResponse(
                todo.Id,
                todo.Title.Value,
                todo.DueDate.Value,
                todo.IsStarred,
                todo.CompletionStatus.ToString()
            ))
            .ToList();

        return Result.Success(response);
    }
}