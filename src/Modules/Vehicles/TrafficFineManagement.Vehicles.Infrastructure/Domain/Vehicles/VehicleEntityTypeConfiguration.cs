using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Domain.Vehicles;

internal sealed class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles", "vehicles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Property(x => x.Plaka)
            .HasField("_plaka")
            .HasColumnName("Plaka");
        builder.Property(x => x.Brand)
            .HasField("_brand")
            .HasColumnName("Brand");
        builder.Property(x => x.Model)
            .HasField("_model")
            .HasColumnName("Model");
        builder.Property(x => x.Status)
            .HasField("_status")
            .HasColumnName("Status");

        builder.Ignore(x => x.Users);
        builder.OwnsMany<VehicleUser>("_users", user =>
        {
            user.WithOwner().HasForeignKey("VehicleId");
            user.ToTable("VehicleUsers", "vehicles");
            user.Property(x => x.UserId).HasColumnName("UserId");
            user.Property(x => x.StartTime)
                .HasField("_startTime")
                .HasColumnName("StartTime");
            user.Property(x => x.EndTime)
                .HasField("_endTime")
                .HasColumnName("EndTime");
            user.HasKey("VehicleId", nameof(VehicleUser.UserId));
        });
    }
}
