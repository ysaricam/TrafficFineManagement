using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TrafficFineManagement.BuildingBlocks.Infrastructure.Serialization;

public sealed class AllPropertiesContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(
        MemberInfo member,
        MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        property.Writable = true;

        return property;
    }
}
