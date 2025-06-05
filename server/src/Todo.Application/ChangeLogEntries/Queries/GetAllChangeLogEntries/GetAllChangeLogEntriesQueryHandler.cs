using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;
using Todo.Domain.Errors;

namespace Todo.Application.ChangeLogEntries.Queries.GetAllChangeLogEntries;

public sealed class GetAllChangeLogEntriesQueryHandler
    : IQueryHandler<GetAllChangeLogEntriesQuery, List<GetAllChangeLogEntriesResponse>>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetAllChangeLogEntriesQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<List<GetAllChangeLogEntriesResponse>>> Handle(
        GetAllChangeLogEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure<List<GetAllChangeLogEntriesResponse>>(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var response = todoItem.ChangeLogEntries
            .Select(entry => new GetAllChangeLogEntriesResponse(
                entry.Id,
                entry.Text.Value,
                entry.CreatedOnUtc
            ))
            .ToList();

        return Result.Success(response);
    }
}