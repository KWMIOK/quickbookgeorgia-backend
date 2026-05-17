using System.Globalization;
using QuickBookGeorgia.API.DTOs;
using QuickBookGeorgia.API.Entities;
using QuickBookGeorgia.API.Repositories;

namespace QuickBookGeorgia.API.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookings;
    private readonly IServiceRepository _services;
    private readonly IBusinessRepository _businesses;
    private readonly IWhatsAppService _whatsApp;

    public BookingService(
        IBookingRepository bookings,
        IServiceRepository services,
        IBusinessRepository businesses,
        IWhatsAppService whatsApp)
    {
        _bookings = bookings;
        _services = services;
        _businesses = businesses;
        _whatsApp = whatsApp;
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request, CancellationToken ct = default)
    {
        var business = await _businesses.GetByIdAsync(request.BusinessId, ct)
            ?? throw new KeyNotFoundException("Business not found.");

        var service = await _services.GetByIdAsync(request.ServiceId, ct)
            ?? throw new KeyNotFoundException("Service not found.");

        if (service.BusinessId != business.Id)
            throw new InvalidOperationException("Service does not belong to this business.");

        if (!DateOnly.TryParseExact(request.SelectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            throw new ArgumentException("SelectedDate must be in format yyyy-MM-dd.");

        if (!TimeOnly.TryParseExact(request.SelectedTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
            throw new ArgumentException("SelectedTime must be in format HH:mm.");

        if (date < DateOnly.FromDateTime(DateTime.UtcNow.Date))
            throw new InvalidOperationException("Cannot book a date in the past.");

        var workingHours = WorkingHoursHelper.Deserialize(business.WorkingHoursJson);
        var dayHours = WorkingHoursHelper.GetForDay(workingHours, date.DayOfWeek);
        if (dayHours is null)
            throw new InvalidOperationException("Business is closed on this day.");

        var open = TimeOnly.ParseExact(dayHours.Open, "HH:mm", CultureInfo.InvariantCulture);
        var close = TimeOnly.ParseExact(dayHours.Close, "HH:mm", CultureInfo.InvariantCulture);

        var slotEnd = time.AddMinutes(service.DurationMinutes);
        if (time < open || slotEnd > close)
            throw new InvalidOperationException("Selected time is outside working hours.");

        if (await _bookings.ExistsAsync(business.Id, date, time, ct))
            throw new InvalidOperationException("This time slot is already booked.");

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            BusinessId = business.Id,
            ServiceId = service.Id,
            CustomerName = request.CustomerName.Trim(),
            CustomerPhone = request.CustomerPhone.Trim(),
            SelectedDate = date,
            SelectedTime = time,
            CreatedAt = DateTime.UtcNow,
        };

        await _bookings.AddAsync(booking, ct);
        await _bookings.SaveChangesAsync(ct);

        var waUrl = _whatsApp.BuildBookingLink(
            business.PhoneNumber,
            service.Name,
            date.ToString("yyyy-MM-dd"),
            time.ToString("HH:mm"),
            booking.CustomerName,
            booking.CustomerPhone);

        return new BookingResponse
        {
            Id = booking.Id,
            BusinessId = booking.BusinessId,
            ServiceId = booking.ServiceId,
            ServiceName = service.Name,
            CustomerName = booking.CustomerName,
            CustomerPhone = booking.CustomerPhone,
            SelectedDate = date.ToString("yyyy-MM-dd"),
            SelectedTime = time.ToString("HH:mm"),
            CreatedAt = booking.CreatedAt,
            WhatsAppUrl = waUrl,
        };
    }

    public async Task<AvailableSlotsResponse> GetAvailableSlotsAsync(
        Guid businessId, Guid serviceId, string date, CancellationToken ct = default)
    {
        var business = await _businesses.GetByIdAsync(businessId, ct)
            ?? throw new KeyNotFoundException("Business not found.");

        var service = await _services.GetByIdAsync(serviceId, ct)
            ?? throw new KeyNotFoundException("Service not found.");

        if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            throw new ArgumentException("date must be in format yyyy-MM-dd.");

        var workingHours = WorkingHoursHelper.Deserialize(business.WorkingHoursJson);
        var dayHours = WorkingHoursHelper.GetForDay(workingHours, d.DayOfWeek);

        var result = new AvailableSlotsResponse { Date = date };
        if (dayHours is null) return result;

        var open = TimeOnly.ParseExact(dayHours.Open, "HH:mm", CultureInfo.InvariantCulture);
        var close = TimeOnly.ParseExact(dayHours.Close, "HH:mm", CultureInfo.InvariantCulture);
        var interval = business.SlotIntervalMinutes <= 0 ? 30 : business.SlotIntervalMinutes;

        var existing = await _bookings.GetForBusinessOnDateAsync(businessId, d, ct);
        var takenTimes = existing.Select(b => b.SelectedTime).ToHashSet();

        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now.Date);

        var current = open;
        while (current.AddMinutes(service.DurationMinutes) <= close)
        {
            var isPast = d == today && current <= TimeOnly.FromDateTime(now);
            if (!takenTimes.Contains(current) && !isPast)
            {
                result.Slots.Add(current.ToString("HH:mm"));
            }
            current = current.AddMinutes(interval);
        }

        return result;
    }
}
