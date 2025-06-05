using Todo.Domain.DomainEvents.NoteDomainEvents;
using Todo.Domain.DomainEvents.TodoItemDomainEvents;
using Todo.Domain.Enums;
using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;
using Todo.Domain.ValueObjects.ChangeLogEntry;
using Todo.Domain.ValueObjects.Note;
using Todo.Domain.ValueObjects.TodoItem;

namespace Todo.Domain.Entities;

public class TodoItem : AggregateRoot
{
    private readonly List<Note> _notes = [];
    private readonly List<ChangeLogEntry> _changeLogEntries = [];

    public Guid UserId { get; private set; }
    public TodoTitle Title { get; private set; }
    public CompletionStatus CompletionStatus { get; private set; }
    public DueDate DueDate { get; private set; }
    public bool IsStarred { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? LastModifiedOnUtc { get; private set; }

    public IReadOnlyCollection<Note> Notes => _notes.AsReadOnly();
    public IReadOnlyCollection<ChangeLogEntry> ChangeLogEntries => _changeLogEntries.AsReadOnly();

    private TodoItem() { }

    private TodoItem(Guid id, Guid userId, TodoTitle title, DueDate dueDate)
        : base(id)
    {
        UserId = userId;
        Title = title;
        CompletionStatus = CompletionStatus.Active;
        DueDate = dueDate;
        IsStarred = false;
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = null;
    }

    public static TodoItem Create(Guid userId, TodoTitle title, DueDate dueDate)
    {
        var todoItem = new TodoItem(Guid.NewGuid(), userId, title, dueDate);

        todoItem.RaiseDomainEvent(new TodoItemCreatedDomainEvent(Guid.NewGuid(), todoItem.Id));

        return todoItem;
    }

    public void UpdateTitle(TodoTitle newTitle)
    {
        var oldTitle = Title.Value;

        Title = newTitle;
        LastModifiedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(new TodoItemTitleUpdatedDomainEvent(new Guid(), Id, oldTitle));
    }

    public void UpdateStatus(CompletionStatus status)
    {
        var oldStatus = CompletionStatus.ToString();

        CompletionStatus = status;
        LastModifiedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(new TodoItemStatusUpdatedDomainEvent(new Guid(), Id, oldStatus));
    }

    public void UpdateDueDate(DueDate newDueDate)
    {
        var oldDueDate = newDueDate.Value;

        DueDate = newDueDate;
        LastModifiedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(new TodoItemDueDateUpdatedDomainEvent(new Guid(), Id, oldDueDate, LastModifiedOnUtc.Value));
    }

    public void UpdateIsStarred(bool isStarred)
    {
        IsStarred = isStarred;
        LastModifiedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(new TodoItemIsStarredUpdatedDomainEvent(new Guid(), Id, isStarred)); // the new value?
    }

    public Guid CreateAndAddNote(NoteText text)
    {
        var note = Note.Create(Id, text);

        _notes.Add(note);

        RaiseDomainEvent(new NoteCreatedDomainEvent(new Guid(), Id, note.Text.Value, note.CreatedOnUtc));

        return note.Id;
    }

    public Result UpdateNoteText(Guid noteId, NoteText newText)
    {
        var note = _notes.FirstOrDefault(n => n.Id == noteId);

        if (note is null)
        {
            return Result.Failure(DomainErrors.Todo.NoteNotFound);
        }

        var oldText = note.Text.Value;

        note.UpdateText(newText);

        LastModifiedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(new NoteTextUpdatedDomainEvent(new Guid(), Id, oldText, note.Text.Value, LastModifiedOnUtc.Value));

        return Result.Success();
    }

    public Result RemoveNote(Guid noteId)
    {
        var note = _notes.FirstOrDefault(n => n.Id == noteId);

        if (note is null)
        {
            return Result.Failure(DomainErrors.Todo.NoteNotFound);
        }

        _notes.Remove(note);

        RaiseDomainEvent(new NoteRemovedDomainEvent(new Guid(), Id, note.Text.Value, DateTime.Now));

        return Result.Success();
    }

    public Guid CreateAndAddChangeLogEntry(ChangeLogEntryText text, DateTime createOnUtc)
    {
        var changeLogEntry = ChangeLogEntry.Create(Id, text, createOnUtc);

        _changeLogEntries.Add(changeLogEntry);

        return changeLogEntry.Id;
    }
}