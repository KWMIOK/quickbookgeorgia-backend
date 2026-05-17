using System.Text.Json;
using QuickBookGeorgia.API.DTOs;

namespace QuickBookGeorgia.API.Services;

public static class WorkingHoursHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static Dictionary<string, DayHours?> Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json) || json == "{}")
            return new Dictionary<string, DayHours?>();

        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, DayHours?>>(json, Options);
            return dict ?? new Dictionary<string, DayHours?>();
        }
        catch
        {
            return new Dictionary<string, DayHours?>();
        }
    }

    public static string Serialize(Dictionary<string, DayHours?>? hours)
    {
        if (hours is null || hours.Count == 0) return "{}";
        var normalized = hours.ToDictionary(
            kv => kv.Key.ToLowerInvariant(),
            kv => kv.Value);
        return JsonSerializer.Serialize(normalized);
    }

    public static DayHours? GetForDay(Dictionary<string, DayHours?> hours, DayOfWeek dayOfWeek)
    {
        var key = dayOfWeek.ToString().ToLowerInvariant();
        return hours.TryGetValue(key, out var v) ? v : null;
    }

    public static Dictionary<string, DayHours?> DefaultWeek() => new()
    {
        ["monday"]    = new DayHours { Open = "09:00", Close = "18:00" },
        ["tuesday"]   = new DayHours { Open = "09:00", Close = "18:00" },
        ["wednesday"] = new DayHours { Open = "09:00", Close = "18:00" },
        ["thursday"]  = new DayHours { Open = "09:00", Close = "18:00" },
        ["friday"]    = new DayHours { Open = "09:00", Close = "18:00" },
        ["saturday"]  = new DayHours { Open = "10:00", Close = "16:00" },
        ["sunday"]    = null,
    };
}
