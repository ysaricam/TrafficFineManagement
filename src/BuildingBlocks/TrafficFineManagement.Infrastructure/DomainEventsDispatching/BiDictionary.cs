using System.Diagnostics.CodeAnalysis;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public sealed class BiDictionary<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull
{
    private readonly Dictionary<TFirst, TSecond> _firstToSecond = [];
    private readonly Dictionary<TSecond, TFirst> _secondToFirst = [];

    public IReadOnlyCollection<TSecond> SecondValues => _secondToFirst.Keys;

    public void Add(TFirst first, TSecond second)
    {
        if (_firstToSecond.ContainsKey(first))
        {
            throw new ArgumentException("The first value is already registered.", nameof(first));
        }

        if (_secondToFirst.ContainsKey(second))
        {
            throw new ArgumentException("The second value is already registered.", nameof(second));
        }

        _firstToSecond.Add(first, second);
        _secondToFirst.Add(second, first);
    }

    public bool TryGetByFirst(
        TFirst first,
        [MaybeNullWhen(false)] out TSecond second)
    {
        return _firstToSecond.TryGetValue(first, out second);
    }

    public bool TryGetBySecond(
        TSecond second,
        [MaybeNullWhen(false)] out TFirst first)
    {
        return _secondToFirst.TryGetValue(second, out first);
    }
}
