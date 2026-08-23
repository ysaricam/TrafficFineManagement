using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Domain.Users;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "vehicles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Role)
            .HasField("_role")
            .HasColumnName("Role");
    }
}
