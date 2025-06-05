using MediatR;
using Todo.Domain.DomainEvents.NoteDomainEvents;
using Todo.Domain.Repositories;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Application.Notes.EventHandlers.DeletedEventHandlers;

public sealed class NoteRemovedDomainEventHandler : INotificationHandler<NoteRemovedDomainEvent>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NoteRemovedDomainEventHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(NoteRemovedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(domainEvent.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return;
        }

        var message = $"The note: '{domainEvent.NoteText}' was deleted from this task: '{todoItem.Title}'.";

        var textResult = ChangeLogEntryText.Create(message);

        if (textResult.IsFailure)
        {
            return;
        }

        todoItem.CreateAndAddChangeLogEntry(textResult.Value, domainEvent.CreatedOnUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}