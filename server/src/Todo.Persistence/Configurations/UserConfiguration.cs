using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.ValueObjects.User;

namespace Todo.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasConversion(f => f.Value, v => FirstName.Create(v).Value)
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .HasConversion(l => l.Value, v => LastName.Create(v).Value)
            .HasMaxLength(50);

        builder.Property(u => u.Address)
            .HasConversion(a => a.Value, v => Address.Create(v).Value)
            .HasMaxLength(200);
    }
}