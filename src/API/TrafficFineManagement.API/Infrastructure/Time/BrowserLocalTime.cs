namespace TrafficFineManagement.API.Infrastructure.Time;

public static class BrowserLocalTime
{
    public static DateTime ToUtc(
        DateTime localDateTime,
        int? timeZoneOffsetMinutes)
    {
        if (!timeZoneOffsetMinutes.HasValue)
        {
            return DateTime.SpecifyKind(localDateTime, DateTimeKind.Utc);
        }

        if (timeZoneOffsetMinutes is < -840 or > 840)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeZoneOffsetMinutes));
        }

        var unspecifiedLocalTime = DateTime.SpecifyKind(
            localDateTime,
            DateTimeKind.Unspecified);

        return DateTime.SpecifyKind(
            unspecifiedLocalTime.AddMinutes(timeZoneOffsetMinutes.Value),
            DateTimeKind.Utc);
    }
}
