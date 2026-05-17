namespace QuickBookGeorgia.API.Services;

public interface IWhatsAppService
{
    string BuildBookingLink(
        string businessPhone,
        string serviceName,
        string date,
        string time,
        string customerName,
        string customerPhone);
}
