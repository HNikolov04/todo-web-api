using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.ValueObjects.TodoItem;

namespace Todo.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .HasConversion(title => title.Value, value => TodoTitle.Create(value).Value)
            .HasMaxLength(TodoTitle.MaxLength)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .HasConversion(d => d.Value, v => DueDate.Create(v).Value)
            .IsRequired();

        builder.Property(t => t.CompletionStatus)
            .HasConversion<int>();

        builder.HasMany(t => t.Notes)
            .WithOne()
            .HasForeignKey(n => n.TodoItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.ChangeLogEntries)
            .WithOne()
            .HasForeignKey(c => c.TodoItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}