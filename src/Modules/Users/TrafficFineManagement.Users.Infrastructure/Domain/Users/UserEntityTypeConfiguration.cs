using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Modules.Users.Domain.Users;

namespace TrafficFineManagement.Modules.Users.Infrastructure.Domain.Users;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasField("_id")
            .HasColumnName("Id")
            .ValueGeneratedNever();
        builder.Property(x => x.Name)
            .HasField("_name")
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Surname)
            .HasField("_surname")
            .HasColumnName("Surname")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Username)
            .HasField("_username")
            .HasColumnName("Username")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(x => x.PasswordHash)
            .HasField("_passwordHash")
            .HasColumnName("PasswordHash")
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(x => x.Role)
            .HasField("_role")
            .HasColumnName("Role")
            .IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();
    }
}
