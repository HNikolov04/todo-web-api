using Newtonsoft.Json;
using Todo.Domain.Primitives;
using Todo.Domain.Repositories;
using Todo.Persistence.Outbox;

namespace Todo.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();

        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var domainEventHolders = _dbContext.ChangeTracker
            .Entries()
            .Where(e => e.Entity is IDomainEventHolder)
            .Select(e => e.Entity as IDomainEventHolder)
            .Where(e => e is not null)!;

        var outboxMessages = domainEventHolders
            .SelectMany(holder =>
            {
                var events = holder.GetDomainEvents();
                holder.ClearDomainEvents();
                return events;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        _dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}