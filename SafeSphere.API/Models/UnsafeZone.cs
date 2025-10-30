using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.Models;

/// <summary>
/// Represents an unsafe zone marked on the map
/// Can be based on user reports or pre-loaded crime data
/// </summary>
public class UnsafeZone
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public double CenterLat { get; set; }

    [Required]
    public double CenterLng { get; set; }

    /// <summary>
    /// Radius in meters defining the unsafe zone area
    /// </summary>
    [Required]
    public double RadiusMeters { get; set; } = 500; // Default 500 meters

    /// <summary>
    /// Severity level: Low, Medium, High, Critical
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Severity { get; set; } = "Medium";

    /// <summary>
    /// Type of threat: Crime, Accident, Natural, Construction, Other
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ThreatType { get; set; } = "Other";

    /// <summary>
    /// Status: Active, Resolved, Expired
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// ID of user who reported this zone (null if pre-loaded)
    /// </summary>
    public int? ReportedByUserId { get; set; }

    /// <summary>
    /// Number of user confirmations for this zone
    /// </summary>
    public int ConfirmationCount { get; set; } = 0;

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }
}

