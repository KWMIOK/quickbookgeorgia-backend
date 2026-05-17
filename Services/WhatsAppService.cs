using System.Text;

namespace QuickBookGeorgia.API.Services;

public class WhatsAppService : IWhatsAppService
{
    public string BuildBookingLink(
        string businessPhone,
        string serviceName,
        string date,
        string time,
        string customerName,
        string customerPhone)
    {
        var phoneDigits = NormalizePhone(businessPhone);

        var message = new StringBuilder()
            .AppendLine("New Booking:")
            .AppendLine($"Service: {serviceName}")
            .AppendLine($"Date: {date}")
            .AppendLine($"Time: {time}")
            .AppendLine($"Customer: {customerName}")
            .Append($"Phone: {customerPhone}")
            .ToString();

        var encoded = Uri.EscapeDataString(message);
        return $"https://wa.me/{phoneDigits}?text={encoded}";
    }

    /// <summary>wa.me requires digits only, no +, no spaces, no dashes.</summary>
    private static string NormalizePhone(string phone)
    {
        var sb = new StringBuilder(phone.Length);
        foreach (var c in phone)
        {
            if (char.IsDigit(c)) sb.Append(c);
        }
        return sb.ToString();
    }
}
