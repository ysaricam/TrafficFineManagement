using TrafficFineManagement.BuildingBlocks.Domain;
using TrafficFineManagement.Modules.Users.Domain.Users.Events;

namespace TrafficFineManagement.Modules.Users.Domain.Users;

public sealed class User : Entity, IAggregateRoot
{
    private UserId _id = null!;
    private string _name = string.Empty;
    private string _surname = string.Empty;
    private string _username = string.Empty;
    private string _passwordHash = string.Empty;
    private UserRole _role;

    private User()
    {
    }

    private User(
        Guid id,
        string name,
        string surname,
        string username,
        string passwordHash,
        UserRole role)
    {
        if (!Enum.IsDefined(role))
        {
            throw new ArgumentOutOfRangeException(nameof(role));
        }

        _id = new UserId(id);
        _name = GetRequiredValue(name, nameof(name));
        _surname = GetRequiredValue(surname, nameof(surname));
        _username = GetRequiredValue(username, nameof(username));
        _passwordHash = GetRequiredValue(passwordHash, nameof(passwordHash));
        _role = role;

        AddDomainEvent(new UserCreatedDomainEvent(
            _id,
            _name,
            _surname,
            _username,
            _role));
    }

    public UserId Id => _id;

    public string Name => _name;

    public string Surname => _surname;

    public string Username => _username;

    public string PasswordHash => _passwordHash;

    public UserRole Role => _role;

    public static User Create(
        Guid id,
        string name,
        string surname,
        string username,
        string passwordHash,
        UserRole role)
    {
        return new User(id, name, surname, NormalizeUsername(username), passwordHash, role);
    }

    private static string GetRequiredValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty.", parameterName);
        }

        return value.Trim();
    }

    private static string NormalizeUsername(string username)
    {
        return GetRequiredValue(username, nameof(username)).ToLowerInvariant();
    }
}
