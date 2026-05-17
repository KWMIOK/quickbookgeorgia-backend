using Microsoft.EntityFrameworkCore;
using QuickBookGeorgia.API.Entities;
using QuickBookGeorgia.API.Services;

namespace QuickBookGeorgia.API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Businesses.AnyAsync()) return;

        var barber = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Giorgi Barber",
            Slug = "giorgi-barber",
            PhoneNumber = "+995555123456",
            Description = "Modern barbershop in the heart of Tbilisi. Walk-ins welcome.",
            WorkingHoursJson = WorkingHoursHelper.Serialize(WorkingHoursHelper.DefaultWeek()),
            SlotIntervalMinutes = 30,
            CreatedAt = DateTime.UtcNow,
            Services = new List<Service>
            {
                new() { Id = Guid.NewGuid(), Name = "Haircut", DurationMinutes = 30, Price = 30m, IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Beard Trim", DurationMinutes = 20, Price = 20m, IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Haircut + Beard", DurationMinutes = 60, Price = 45m, IsActive = true },
            }
        };

        var salon = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Nino Beauty Salon",
            Slug = "nino-beauty",
            PhoneNumber = "+995577998877",
            Description = "Full-service beauty salon — hair, nails, brows.",
            WorkingHoursJson = WorkingHoursHelper.Serialize(WorkingHoursHelper.DefaultWeek()),
            SlotIntervalMinutes = 30,
            CreatedAt = DateTime.UtcNow,
            Services = new List<Service>
            {
                new() { Id = Guid.NewGuid(), Name = "Manicure", DurationMinutes = 45, Price = 35m, IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Pedicure", DurationMinutes = 60, Price = 50m, IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Hair Color", DurationMinutes = 120, Price = 120m, IsActive = true },
            }
        };

        await db.Businesses.AddRangeAsync(barber, salon);
        await db.SaveChangesAsync();
    }
}
