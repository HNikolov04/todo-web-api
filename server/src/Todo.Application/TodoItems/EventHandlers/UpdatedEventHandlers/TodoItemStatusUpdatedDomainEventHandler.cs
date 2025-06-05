using MediatR;
using Todo.Domain.DomainEvents.TodoItemDomainEvents;
using Todo.Domain.Repositories;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Application.TodoItems.EventHandlers.UpdatedEventHandlers;

public sealed class TodoItemStatusUpdatedDomainEventHandler : INotificationHandler<TodoItemStatusUpdatedDomainEvent>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TodoItemStatusUpdatedDomainEventHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TodoItemStatusUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(domainEvent.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return;
        }

        var message = $"Todo status changed from '{domainEvent.OldStatus}' to '{todoItem.CompletionStatus}'.";

        var textResult = ChangeLogEntryText.Create(message);

        if (textResult.IsFailure)
        {
            return;
        }

        todoItem.CreateAndAddChangeLogEntry(textResult.Value, todoItem.CreatedOnUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}