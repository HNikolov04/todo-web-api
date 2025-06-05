using Todo.Domain.Primitives;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Domain.Entities;

public class ChangeLogEntry : Entity
{
    public Guid TodoItemId { get; private set; }
    public ChangeLogEntryText Text { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    private ChangeLogEntry() { }

    private ChangeLogEntry(Guid id, Guid todoItemId, ChangeLogEntryText text, DateTime createOnUtc)
        : base(id)
    {
        TodoItemId = todoItemId;
        Text = text;
        CreatedOnUtc = createOnUtc;
    }

    public static ChangeLogEntry Create(Guid todoItemId, ChangeLogEntryText text, DateTime createdOnUtc)
    {
        return new ChangeLogEntry(Guid.NewGuid(), todoItemId, text, createdOnUtc);
    }
}