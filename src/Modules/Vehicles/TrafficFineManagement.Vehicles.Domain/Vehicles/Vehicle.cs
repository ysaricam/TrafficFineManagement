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
    private VehicleType _type;
    private readonly List<VehicleUser> _users;
    private bool _status;
    private DateTime _lastModifiedAt;

    public string Plaka => _plaka;
    public string Brand => _brand;
    public string Model => _model;
    public VehicleType Type => _type;
    public bool Status => _status;
    public DateTime LastModifiedAt => _lastModifiedAt;
    public IReadOnlyCollection<VehicleUser> Users => _users.AsReadOnly();

    private Vehicle()
    {
        _status = false;
        _lastModifiedAt = DateTime.UtcNow;
        _users = [];
    }

    public static Vehicle Create(
        string plaka,
        string brand,
        string model,
        VehicleType type)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plaka);
        ArgumentException.ThrowIfNullOrWhiteSpace(brand);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type));
        }

        return new Vehicle(plaka, brand, model, type);
    }

    private Vehicle(
        string plaka,
        string brand,
        string model,
        VehicleType type)
    {
        Id = new VehicleId(Guid.NewGuid());
        _plaka = plaka;
        _brand = brand;
        _model = model;
        _type = type;

        _status = false;
        _lastModifiedAt = DateTime.UtcNow;
        _users = [];

        AddDomainEvent(new VehicleCreatedDomainEvent(Id));
    }


    public void AddUser(UserId id, DateTime startTime)
    {
        if (_status) return;

        var vehicleUser = VehicleUser.Create(id, startTime);
        _users.Add(vehicleUser);

        _status = true;
        _lastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new VehicleAddUserDomainEvent(Id, id, vehicleUser.StartTime));
        AddDomainEvent(new VehicleStatusUpdatedDomainEvent(Id, _status));
    }

    public void UpdateStatus(UserId id, DateTime endTime)
    {
        var user = _users.FirstOrDefault(x =>
            x.UserId == id && x.EndTime is null);

        if (user is null) return;

        user.Complete(endTime);
        _status = false;
        _lastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new VehicleStatusUpdatedDomainEvent(Id, _status));
    }
}
