using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;

namespace Todo.Domain.ValueObjects.TodoItem;

public sealed class DueDate : ValueObject
{
    public DateTime Value { get; }

    private DueDate(DateTime value)
    {
        Value = value;
    }

    public static Result<DueDate> Create(DateTime dueDate)
    {
        if (dueDate < DateTime.UtcNow)
        {
            return Result.Failure<DueDate>(DomainErrors.Todo.InvalidDueDate);
        }

        return Result.Success(new DueDate(dueDate));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}