using MediatR;
using Todo.Domain.Primitives;

namespace Todo.Application.Abstractions.DomainEventHandler;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
