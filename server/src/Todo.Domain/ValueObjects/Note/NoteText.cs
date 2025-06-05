using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;

namespace Todo.Domain.ValueObjects.Note;

public sealed class NoteText : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; }

    private NoteText(string value)
    {
        Value = value;
    }

    public static Result<NoteText> Create(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Result.Failure<NoteText>(DomainErrors.Note.EmptyText);
        }

        if (text.Length > MaxLength)
        {
            return Result.Failure<NoteText>(DomainErrors.Note.TooLong);
        }

        return Result.Success(new NoteText(text));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}