using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.Vehicles.Domain.Users;
using TrafficFineManagement.Modules.Vehicles.Domain.Vehicles.Events;

namespace TrafficFineManagement.Modules.Vehicles.Domain.Vehicles;

public class Vehicle : Entity, IAggregateRoot
{
    public VehicleId Id { get; private set; } = null!;
    private string _plaka = string.Empty;
    private string _brand = string.Empty;
    private string _model = string.Empty;
    private readonly List<VehicleUser> _users;
    private bool _status;

    public string Plaka => _plaka;
    public string Brand => _brand;
    public string Model => _model;
    public bool Status => _status;
    public IReadOnlyCollection<VehicleUser> Users => _users.AsReadOnly();

    private Vehicle()
    {
        _status = false;
        _users = [];
    }

    public static Vehicle Create(string plaka, string brand, string model)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plaka);
        ArgumentException.ThrowIfNullOrWhiteSpace(brand);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        return new Vehicle(plaka, brand, model);
    }

    private Vehicle(string plaka, string brand, string model)
    {
        Id = new VehicleId(Guid.NewGuid());
        _plaka = plaka;
        _brand = brand;
        _model = model;

        _status = false;
        _users = [];

        AddDomainEvent(new VehicleCreatedDomainEvent(Id));
    }


    public void AddUser(UserId id, DateTime startTime)
    {
        if (_status) return;

        _users.Add(VehicleUser.Create(id, startTime));

        _status = true;

        AddDomainEvent(new VehicleAddUserDomainEvent(Id, id));
        AddDomainEvent(new VehicleStatusUpdated(Id, _status));
    }

    public void UpdateStatus(UserId id, DateTime endTime)
    {
        var user = _users.FirstOrDefault(x => x.UserId == id);

        if (user is null) return;

        user.Complete(endTime);
        _status = false;

        AddDomainEvent(new VehicleStatusUpdated(Id, _status));
    }
}
