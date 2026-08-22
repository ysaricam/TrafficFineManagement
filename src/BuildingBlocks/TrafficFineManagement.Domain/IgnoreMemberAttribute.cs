namespace TrafficFineManagement.BuildingBlocks.Domain;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class IgnoreMemberAttribute : Attribute
{
}