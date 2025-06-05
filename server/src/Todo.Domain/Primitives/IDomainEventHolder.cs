namespace Todo.Domain.Primitives;

public interface IDomainEventHolder
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}