using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;

namespace Todo.Domain.ValueObjects.TodoItem;

public sealed class TodoTitle : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private TodoTitle(string value)
    {
        Value = value;
    }

    public static Result<TodoTitle> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Failure<TodoTitle>(DomainErrors.Todo.EmptyTitle);
        }

        if (title.Length > MaxLength)
        {
            return Result.Failure<TodoTitle>(DomainErrors.Todo.TitleTooLong);
        }

        return Result.Success(new TodoTitle(title));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}