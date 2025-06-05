using MediatR;
using Todo.Domain.DomainEvents.TodoItemDomainEvents;
using Todo.Domain.Repositories;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Application.TodoItems.EventHandlers.UpdatedEventHandlers;

public sealed class TodoItemDueDateUpdatedDomainEventHandler : INotificationHandler<TodoItemDueDateUpdatedDomainEvent>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TodoItemDueDateUpdatedDomainEventHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TodoItemDueDateUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(domainEvent.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return;
        }

        var message =
            $"Due date was updated from '{domainEvent.OldDateTime:yyyy-MM-dd}' to '{todoItem.DueDate.Value:yyyy-MM-dd}'.";

        var textResult = ChangeLogEntryText.Create(message);

        if (textResult.IsFailure)
        {
            return;
        }

        todoItem.CreateAndAddChangeLogEntry(textResult.Value, domainEvent.CreatedOnUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}