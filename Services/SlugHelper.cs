using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickBookGeorgia.API.Services;

public static class SlugHelper
{
    public static string Generate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var normalized = input.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(c);
        }

        var stripped = sb.ToString().Normalize(NormalizationForm.FormC);
        stripped = Regex.Replace(stripped, @"[^a-z0-9\s-]", "");
        stripped = Regex.Replace(stripped, @"[\s_-]+", "-").Trim('-');
        if (stripped.Length > 50) stripped = stripped[..50].Trim('-');
        return stripped;
    }
}
