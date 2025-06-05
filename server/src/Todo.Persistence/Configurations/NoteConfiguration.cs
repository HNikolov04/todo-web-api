using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.ValueObjects.Note;

namespace Todo.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .ValueGeneratedNever(); 

        builder.Property(n => n.Text)
            .HasConversion(t => t.Value, v => NoteText.Create(v).Value)
            .IsRequired();
    }
}