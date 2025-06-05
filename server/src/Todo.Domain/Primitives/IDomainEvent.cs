using MediatR;

namespace Todo.Domain.Primitives;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}