namespace TrafficFineManagement.BuildingBlocks.Domain;

public abstract class TypedIdValueBase : IEquatable<TypedIdValueBase>
{
    protected TypedIdValueBase(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidOperationException("Id value cannot be empty!");
        }

        Value = value;
    }

    public Guid Value { get; }

    public override bool Equals(object? obj)
    {
        return obj is TypedIdValueBase other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public bool Equals(TypedIdValueBase? other)
    {
        return Value == other?.Value;
    }

    public static bool operator ==(TypedIdValueBase? obj1, TypedIdValueBase? obj2)
    {
        if (object.Equals(obj1, null))
        {
            return object.Equals(obj2, null);
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(TypedIdValueBase? obj1, TypedIdValueBase? obj2)
    {
        return !(obj1 == obj2);
    }
}