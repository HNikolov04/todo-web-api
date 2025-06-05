using MediatR;
using Todo.Domain.DomainEvents.TodoItemDomainEvents;
using Todo.Domain.Repositories;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Application.TodoItems.EventHandlers.CreatedEventHandlers;

public sealed class TodoItemCreatedDomainEventHandler : INotificationHandler<TodoItemCreatedDomainEvent>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TodoItemCreatedDomainEventHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TodoItemCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(domainEvent.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return;
        }

        var message =
            $"Todo item '{todoItem.Title.Value}' was created with due date {todoItem.DueDate.Value:yyyy-MM-dd} and completion status of {todoItem.CompletionStatus.ToString()}.";

        var textResult = ChangeLogEntryText.Create(message);

        if (textResult.IsFailure)
        {
            return;
        }

        todoItem.CreateAndAddChangeLogEntry(textResult.Value, todoItem.CreatedOnUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}