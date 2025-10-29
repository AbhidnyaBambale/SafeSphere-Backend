using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? EmergencyContacts { get; set; } // JSON string or comma-separated

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public ICollection<PanicAlert> PanicAlerts { get; set; } = new List<PanicAlert>();
    public ICollection<SOSAlert> SOSAlerts { get; set; } = new List<SOSAlert>();
}

