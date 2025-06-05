using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Entities;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.TodoItem;

namespace Todo.Application.TodoItems.Commands.CreateTodoItem;

public sealed class CreateTodoItemCommandHandler : ICommandHandler<CreateTodoItemCommand, Guid>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoItemCommandHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var createValueObjects = CreateAndValidateValueObjects(request.Title, request.DueDateTime);

        if (createValueObjects.IsFailure)
        {
            return Result.Failure<Guid>(createValueObjects.Errors);
        }

        var todoItem = TodoItem.Create(
            request.UserId,
            createValueObjects.Value.Title,
            createValueObjects.Value.DueDate
        );

        await _todoItemRepository.AddAsync(todoItem, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(todoItem.Id);
    }

    private static Result<(TodoTitle Title, DueDate DueDate)> CreateAndValidateValueObjects(string title, DateTime dueDateTime)
    {
        var titleResult = TodoTitle.Create(title);
        var dueDateResult = DueDate.Create(dueDateTime);

        if (titleResult.IsFailure || dueDateResult.IsFailure)
        {
            var errors = titleResult.Errors
                .Concat(dueDateResult.Errors)
                .ToArray();

            return Result.Failure<(TodoTitle, DueDate)>(errors);
        }

        return Result.Success((titleResult.Value, dueDateResult.Value));
    }
}