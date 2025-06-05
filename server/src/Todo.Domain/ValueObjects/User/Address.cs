using Todo.Domain.Errors;
using Todo.Domain.Primitives;
using Todo.Domain.Shared;

namespace Todo.Domain.ValueObjects.User;

public class Address : ValueObject
{
    public const int MaxLength = 256;

    private Address(string value)
    {
        Value = value;
    }

    private Address()
    {
    }

    public string Value { get; private set; }

    public static Result<Address> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<Address>(DomainErrors.Address.Empty);
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<Address>(DomainErrors.Address.TooLong);
        }

        return new Address(firstName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}