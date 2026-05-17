using Microsoft.EntityFrameworkCore;
using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Business>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Slug).IsUnique();
            b.Property(x => x.WorkingHoursJson).HasColumnType("jsonb");

            b.HasMany(x => x.Services)
                .WithOne(s => s.Business!)
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.Bookings)
                .WithOne(bo => bo.Business!)
                .HasForeignKey(bo => bo.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Service>(s =>
        {
            s.HasKey(x => x.Id);
            s.Property(x => x.Price).HasColumnType("numeric(10,2)");
        });

        modelBuilder.Entity<Booking>(bo =>
        {
            bo.HasKey(x => x.Id);
            bo.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            bo.HasIndex(x => new { x.BusinessId, x.SelectedDate, x.SelectedTime });
        });
    }
}
