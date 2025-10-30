using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.Models;

/// <summary>
/// Represents a disaster or emergency alert
/// Can be from government APIs or manual entry
/// </summary>
public class DisasterAlert
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type of disaster: Earthquake, Flood, Fire, Hurricane, Tornado, Tsunami, etc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string DisasterType { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string AffectedArea { get; set; } = string.Empty;

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    /// <summary>
    /// Radius in kilometers of affected area
    /// </summary>
    public double? AffectedRadiusKm { get; set; }

    /// <summary>
    /// Severity: Low, Moderate, High, Severe, Extreme
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Severity { get; set; } = "Moderate";

    /// <summary>
    /// Status: Active, Resolved, Monitoring
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// External alert ID for deduplication
    /// </summary>
    [MaxLength(100)]
    public string? ExternalAlertId { get; set; }

    /// <summary>
    /// Source: Government, USGS, NOAA, Manual, etc.
    /// </summary>
    [MaxLength(100)]
    public string? Source { get; set; }

    /// <summary>
    /// Number of people who confirmed seeing this disaster
    /// </summary>
    public int ConfirmationCount { get; set; } = 0;

    [MaxLength(2000)]
    public string? SafetyInstructions { get; set; }

    [MaxLength(500)]
    public string? EmergencyContactInfo { get; set; }
}

