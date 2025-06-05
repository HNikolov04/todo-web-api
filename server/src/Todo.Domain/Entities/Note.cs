using Todo.Domain.Primitives;
using Todo.Domain.ValueObjects.Note;

namespace Todo.Domain.Entities;

public class Note : Entity
{
    public Guid TodoItemId { get; private set; }
    public NoteText Text { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    private Note() { }

    private Note(Guid id, Guid todoItemId, NoteText text)
        : base(id)
    {
        TodoItemId = todoItemId;
        Text = text;
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOn = null;
    }

    public static Note Create(Guid todoItemId, NoteText text)
    {
        return new Note(Guid.NewGuid(), todoItemId, text);
    }

    public void UpdateText(NoteText newText)
    {
        Text = newText;
        LastModifiedOn = DateTime.UtcNow;
    }
}