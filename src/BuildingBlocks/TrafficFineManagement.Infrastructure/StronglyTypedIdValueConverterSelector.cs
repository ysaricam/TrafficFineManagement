using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrafficFineManagement.BuildingBlocks.Domain;

namespace TrafficFineManagament.BuildingBlocks.Infrastructure;

public sealed class StronglyTypedIdValueConverterSelector : ValueConverterSelector
{
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters = [];

    public StronglyTypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IEnumerable<ValueConverterInfo> Select(
        Type modelClrType,
        Type? providerClrType = null)
    {
        foreach (var converter in base.Select(modelClrType, providerClrType))
        {
            yield return converter;
        }

        var underlyingModelType = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        if (underlyingProviderType is not null && underlyingProviderType != typeof(Guid))
        {
            yield break;
        }

        if (!typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType))
        {
            yield break;
        }

        var converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType);

        yield return _converters.GetOrAdd(
            (underlyingModelType, typeof(Guid)),
            _ => new ValueConverterInfo(
                modelClrType,
                typeof(Guid),
                valueConverterInfo =>
                    (ValueConverter)Activator.CreateInstance(
                        converterType,
                        valueConverterInfo.MappingHints)!));
    }

    private static Type? UnwrapNullableType(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}