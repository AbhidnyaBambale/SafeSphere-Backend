using Microsoft.EntityFrameworkCore;
using SafeSphere.API.Models;

namespace SafeSphere.API.Data;

public class SafeSphereDbContext : DbContext
{
    public SafeSphereDbContext(DbContextOptions<SafeSphereDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<PanicAlert> PanicAlerts { get; set; }
    public DbSet<SOSAlert> SOSAlerts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.EmergencyContacts).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // PanicAlert configuration
        modelBuilder.Entity<PanicAlert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Active");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.AdditionalInfo).HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithMany(u => u.PanicAlerts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SOSAlert configuration
        modelBuilder.Entity<SOSAlert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Acknowledged).HasDefaultValue(false);

            entity.HasOne(e => e.User)
                .WithMany(u => u.SOSAlerts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed sample data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Users (password: "password123" hashed with BCrypt)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                EmergencyContacts = "Jane Doe:+1234567891,Police:911",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2,
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                Phone = "+1234567892",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                EmergencyContacts = "John Smith:+1234567893,Emergency:911",
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed PanicAlerts
        modelBuilder.Entity<PanicAlert>().HasData(
            new PanicAlert
            {
                Id = 1,
                UserId = 1,
                LocationLat = 40.7128,
                LocationLng = -74.0060,
                Timestamp = DateTime.UtcNow.AddHours(-2),
                Status = "Resolved",
                AdditionalInfo = "False alarm"
            },
            new PanicAlert
            {
                Id = 2,
                UserId = 2,
                LocationLat = 34.0522,
                LocationLng = -118.2437,
                Timestamp = DateTime.UtcNow.AddMinutes(-30),
                Status = "Active",
                AdditionalInfo = "Need help"
            }
        );

        // Seed SOSAlerts
        modelBuilder.Entity<SOSAlert>().HasData(
            new SOSAlert
            {
                Id = 1,
                UserId = 1,
                Message = "Car accident on Highway 101",
                Location = "Highway 101, San Francisco",
                LocationLat = 37.7749,
                LocationLng = -122.4194,
                Timestamp = DateTime.UtcNow.AddHours(-1),
                Acknowledged = true,
                AcknowledgedAt = DateTime.UtcNow.AddMinutes(-45)
            },
            new SOSAlert
            {
                Id = 2,
                UserId = 2,
                Message = "Feeling unsafe, please check on me",
                Location = "Downtown, Los Angeles",
                LocationLat = 34.0522,
                LocationLng = -118.2437,
                Timestamp = DateTime.UtcNow.AddMinutes(-15),
                Acknowledged = false
            }
        );
    }
}

