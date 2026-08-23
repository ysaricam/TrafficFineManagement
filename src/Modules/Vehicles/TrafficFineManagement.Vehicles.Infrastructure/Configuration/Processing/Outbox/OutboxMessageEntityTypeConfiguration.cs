using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.BuildingBlocks.Application.Outbox;

namespace TrafficFineManagement.Modules.Vehicles.Infrastructure.Configuration.Processing.Outbox;

internal sealed class OutboxMessageEntityTypeConfiguration :
    IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages", "vehicles");

        builder.HasKey(message => message.Id);

        builder.Property(message => message.Id)
            .ValueGeneratedNever();

        builder.Property(message => message.OccurredOn)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(message => message.Type)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(message => message.Data)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(message => message.ProcessedDate)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(message => new
        {
            message.ProcessedDate,
            message.OccurredOn
        });
    }
}
