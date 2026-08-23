using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Modules.TrafficFine.Domain.Fines;
using TrafficFineManagement.Modules.TrafficFine.Domain.Users;
using TrafficFineManagement.Modules.TrafficFine.Domain.Vehicles;

namespace TrafficFineManagement.Modules.TrafficFine.Infrastructure.Domain.Fines;

internal sealed class FineEntityTypeConfiguration : IEntityTypeConfiguration<Fine>
{
    public void Configure(EntityTypeBuilder<Fine> builder)
    {
        builder.ToTable("Fines", "traffic_fines");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Property(x => x.FinedUserId)
            .HasField("_finedUserId")
            .HasColumnName("FinedUserId");
        builder.Property(x => x.VehicleId)
            .HasField("_vehicleId")
            .HasColumnName("VehicleId");
        builder.Property(x => x.ViolationCode)
            .HasField("_violationCode")
            .HasColumnName("ViolationCode")
            .HasMaxLength(50);
        builder.Property(x => x.Reason)
            .HasField("_reason")
            .HasColumnName("Reason")
            .HasMaxLength(1000);
        builder.Property(x => x.FineDate)
            .HasField("_fineDate")
            .HasColumnName("FineDate");
        builder.Property(x => x.Status)
            .HasField("_status")
            .HasColumnName("Status");

        builder.OwnsOne(x => x.Amount, amount =>
        {
            amount.Property(x => x.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2);
            amount.Property(x => x.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });
        builder.Navigation(x => x.Amount).HasField("_amount");

        builder.Ignore(x => x.ApprovalHistory);
        builder.OwnsMany<FineApprovalHistory>("_approvalHistory", history =>
        {
            history.ToTable("FineApprovalHistories", "traffic_fines");
            history.WithOwner().HasForeignKey("FineId");

            history.Property<Guid>("Id").ValueGeneratedOnAdd();
            history.HasKey("Id");

            history.Property(x => x.PerformedByUserId)
                .HasColumnName("PerformedByUserId");
            history.Property(x => x.ActionDate)
                .HasColumnName("ActionDate");
            history.Property(x => x.ActionType)
                .HasColumnName("ActionType");
            history.Property(x => x.Description)
                .HasColumnName("Description")
                .HasMaxLength(1000);
            history.Property(x => x.PreviousStatus)
                .HasColumnName("PreviousStatus");
            history.Property(x => x.NewStatus)
                .HasColumnName("NewStatus");

            history.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            history.HasIndex("FineId", nameof(FineApprovalHistory.ActionDate));
            history.HasIndex(x => x.PerformedByUserId);
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.FinedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.FinedUserId);
        builder.HasIndex(x => x.VehicleId);
        builder.HasIndex(x => x.FineDate);
    }
}
