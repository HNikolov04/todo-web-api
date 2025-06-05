using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.ValueObjects.ChangeLogEntry;

namespace Todo.Persistence.Configurations;

public class ChangeLogEntryConfiguration : IEntityTypeConfiguration<ChangeLogEntry>
{
    public void Configure(EntityTypeBuilder<ChangeLogEntry> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Text)
            .HasConversion(t => t.Value, v => ChangeLogEntryText.Create(v).Value)
            .IsRequired();
    }
}