using MediatR;
using Todo.Domain.DomainEvents.NoteDomainEvents;
using Todo.Domain.Repositories;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Application.Notes.EventHandlers.UpdatedEventHandlers;

public sealed class NoteTextUpdatedDomainEventHandler : INotificationHandler<NoteTextUpdatedDomainEvent>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NoteTextUpdatedDomainEventHandler(
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork)
    {
        _todoItemRepository = todoItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(NoteTextUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(domainEvent.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return;
        }

        var message = $"Note text was updated from '{domainEvent.OldNoteText}' to '{domainEvent.NewNoteText}'.";

        var textResult = ChangeLogEntryText.Create(message);

        if (textResult.IsFailure)
        {
            return;
        }

        todoItem.CreateAndAddChangeLogEntry(textResult.Value, domainEvent.CreatedOnUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}