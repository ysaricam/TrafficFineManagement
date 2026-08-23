using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Vehicles;

internal sealed class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles", "traffic_fines");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
    }
}
