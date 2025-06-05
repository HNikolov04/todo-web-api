using MediatR;
using Todo.Domain.DomainEvents.TodoItemDomainEvents;
using Todo.Domain.Repositories;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Application.TodoItems.EventHandlers.UpdatedEventHandlers;

public sealed class TodoItemIsStarredDomainEventHandler : INotificationHandler<TodoItemIsStarredUpdatedDomainEvent>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TodoItemIsStarredDomainEventHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TodoItemIsStarredUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(domainEvent.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return;
        }

        string oldStatus, newStatus;

        const string STARRED = "starred";
        const string UNSTARRED = "unstarred";

        if (!domainEvent.IsStarred)
        {
            oldStatus = STARRED;
            newStatus = UNSTARRED;
        }
        else
        {
            oldStatus = UNSTARRED;
            newStatus = STARRED;
        }

        var message = $"Todo item was changed from '{oldStatus}' to '{newStatus}'.";

        var textResult = ChangeLogEntryText.Create(message);

        if (textResult.IsFailure)
        {
            return;
        }

        todoItem.CreateAndAddChangeLogEntry(textResult.Value, todoItem.CreatedOnUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}