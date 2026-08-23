using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Vehicles.Domain.UnitTests.Vehicles;

public sealed class VehicleDomainEventsTests
{
    [Fact]
    public void Create_ShouldAddVehicleCreatedDomainEvent()
    {
        var vehicle = Vehicle.Create("34 TFM 001", "Toyota", "Corolla");

        var domainEvent = Assert.IsType<VehicleCreatedDomainEvent>(
            Assert.Single(vehicle.DomainEvents!));

        Assert.Equal(vehicle.Id, domainEvent.VehicleId);
        Assert.NotEqual(Guid.Empty, domainEvent.Id);
        Assert.Equal(DateTimeKind.Utc, domainEvent.OccurredOn.Kind);
    }

    [Fact]
    public void AddUser_ShouldAddUserAndStatusUpdatedDomainEvents()
    {
        var vehicle = CreateVehicleWithoutDomainEvents();
        var userId = new UserId(Guid.NewGuid());
        var startTime = DateTime.UtcNow;

        vehicle.AddUser(userId, startTime);

        Assert.Collection(
            vehicle.DomainEvents!,
            domainEvent =>
            {
                var userAdded = Assert.IsType<VehicleAddUserDomainEvent>(domainEvent);
                Assert.Equal(vehicle.Id, userAdded.VehicleId);
                Assert.Equal(userId, userAdded.UserId);
                Assert.Equal(startTime, userAdded.StartTime);
            },
            domainEvent =>
            {
                var statusUpdated = Assert.IsType<VehicleStatusUpdatedDomainEvent>(domainEvent);
                Assert.Equal(vehicle.Id, statusUpdated.VehicleId);
                Assert.True(statusUpdated.Status);
            });
    }

    [Fact]
    public void UpdateStatus_ShouldAddStatusUpdatedDomainEvent()
    {
        var vehicle = CreateVehicleWithoutDomainEvents();
        var userId = new UserId(Guid.NewGuid());
        vehicle.AddUser(userId, DateTime.UtcNow);
        vehicle.ClearDomainEvents();

        vehicle.UpdateStatus(userId, DateTime.UtcNow.AddHours(1));

        var domainEvent = Assert.IsType<VehicleStatusUpdatedDomainEvent>(
            Assert.Single(vehicle.DomainEvents!));

        Assert.Equal(vehicle.Id, domainEvent.VehicleId);
        Assert.False(domainEvent.Status);
    }

    [Fact]
    public void UpdateStatus_WhenEndTimePrecedesStartTime_ShouldBreakBusinessRule()
    {
        var vehicle = CreateVehicleWithoutDomainEvents();
        var userId = new UserId(Guid.NewGuid());
        var startTime = DateTime.UtcNow;
        vehicle.AddUser(userId, startTime);
        vehicle.ClearDomainEvents();

        var exception = Assert.Throws<BusinessRuleValidationException>(() =>
            vehicle.UpdateStatus(userId, startTime.AddMinutes(-1)));

        Assert.Equal(
            "Vehicle usage end time cannot be earlier than its start time.",
            exception.Message);
        Assert.True(vehicle.Status);
        Assert.Empty(vehicle.DomainEvents!);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var vehicle = Vehicle.Create("34 TFM 002", "Honda", "Civic");

        vehicle.ClearDomainEvents();

        Assert.Empty(vehicle.DomainEvents!);
    }

    private static Vehicle CreateVehicleWithoutDomainEvents()
    {
        var vehicle = Vehicle.Create("34 TFM 003", "Ford", "Focus");
        vehicle.ClearDomainEvents();

        return vehicle;
    }
}
