using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.TodoItem;

namespace Todo.Application.TodoItems.Commands.UpdateTodoItem;

public sealed class UpdateTodoItemCommandHandler : ICommandHandler<UpdateTodoItemCommand>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTodoItemCommandHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var updateResult = ApplyUpdates(todoItem, request);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Errors);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result ApplyUpdates(TodoItem todoItem, UpdateTodoItemCommand request)
    {
        if (request.NewTitle is not null)
        {
            var newTitleResult = TodoTitle.Create(request.NewTitle);

            if (newTitleResult.IsFailure)
            {
                return Result.Failure(newTitleResult.Errors);
            }

            todoItem.UpdateTitle(newTitleResult.Value);
        }

        if (request.NewDueDate is not null)
        {
            var newDueDateResult = DueDate.Create(request.NewDueDate.Value);

            if (newDueDateResult.IsFailure)
            {
                return Result.Failure(newDueDateResult.Errors);
            }

            todoItem.UpdateDueDate(newDueDateResult.Value);
        }

        if (request.IsStarred is not null)
        {
            todoItem.UpdateIsStarred(request.IsStarred.Value);
        }

        if (request.NewCompletionStatus is not null)
        {
            todoItem.UpdateStatus(request.NewCompletionStatus.Value);
        }

        return Result.Success();
    }
}