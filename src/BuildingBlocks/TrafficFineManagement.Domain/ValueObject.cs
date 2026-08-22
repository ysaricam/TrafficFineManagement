using System.Reflection;

namespace TrafficFineManagement.BuildingBlocks.Domain;

public abstract class ValueObject : IEquatable<ValueObject>
{
    private List<PropertyInfo>? _properties;

    private List<FieldInfo>? _fields;

    public static bool operator ==(ValueObject? obj1, ValueObject? obj2)
    {
        if (object.Equals(obj1, null))
        {
            return object.Equals(obj2, null);
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(ValueObject? obj1, ValueObject? obj2)
    {
        return !(obj1 == obj2);
    }

    public bool Equals(ValueObject? other)
    {
        return Equals(other as object);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }

        return GetProperties().All(property => PropertiesAreEqual(obj, property))
            && GetFields().All(field => FieldsAreEqual(obj, field));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;

            foreach (var property in GetProperties())
            {
                hash = HashValue(hash, property.GetValue(this, null));
            }

            foreach (var field in GetFields())
            {
                hash = HashValue(hash, field.GetValue(this));
            }

            return hash;
        }
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo property)
    {
        return object.Equals(property.GetValue(this, null), property.GetValue(obj, null));
    }

    private bool FieldsAreEqual(object obj, FieldInfo field)
    {
        return object.Equals(field.GetValue(this), field.GetValue(obj));
    }

    private IEnumerable<PropertyInfo> GetProperties()
    {
        _properties ??= GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetCustomAttribute(typeof(IgnoreMemberAttribute)) is null)
            .ToList();

        return _properties;
    }

    private IEnumerable<FieldInfo> GetFields()
    {
        _fields ??= GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(field => field.GetCustomAttribute(typeof(IgnoreMemberAttribute)) is null)
            .ToList();

        return _fields;
    }

    private static int HashValue(int seed, object? value)
    {
        var currentHash = value?.GetHashCode() ?? 0;

        return (seed * 23) + currentHash;
    }
}