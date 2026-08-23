using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Users;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "traffic_fines");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Role)
            .HasField("_role")
            .HasColumnName("Role")
            .IsRequired();
    }
}
