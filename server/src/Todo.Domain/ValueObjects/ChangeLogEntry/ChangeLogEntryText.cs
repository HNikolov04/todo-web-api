using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;

namespace Todo.Domain.ValueObjects.ChangeLogEntry;

public sealed class ChangeLogEntryText : ValueObject
{
    public const int MaxLength = 300;

    public string Value { get; }

    private ChangeLogEntryText(string value)
    {
        Value = value;
    }

    public static Result<ChangeLogEntryText> Create(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Result.Failure<ChangeLogEntryText>(DomainErrors.ChangeLogEntry.EmptyText);
        }

        if (text.Length > MaxLength)
        {
            return Result.Failure<ChangeLogEntryText>(DomainErrors.ChangeLogEntry.TooLong);
        }

        return Result.Success(new ChangeLogEntryText(text));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}