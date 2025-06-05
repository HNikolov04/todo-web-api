using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Todo.Persistence.Outbox;

namespace Todo.Persistence.Configurations;

public class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.HasKey(x => new { x.Id, x.Name });

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}