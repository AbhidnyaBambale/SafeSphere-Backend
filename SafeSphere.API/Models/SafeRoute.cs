using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeSphere.API.Models;

/// <summary>
/// Represents a calculated safe route between two points
/// Stored for analytics and quick retrieval
/// </summary>
public class SafeRoute
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public double OriginLat { get; set; }

    [Required]
    public double OriginLng { get; set; }

    [Required]
    public double DestinationLat { get; set; }

    [Required]
    public double DestinationLng { get; set; }

    /// <summary>
    /// JSON encoded route coordinates (array of lat/lng points)
    /// </summary>
    [Required]
    public string RouteCoordinates { get; set; } = "[]";

    /// <summary>
    /// Estimated distance in meters
    /// </summary>
    [Required]
    public double DistanceMeters { get; set; }

    /// <summary>
    /// Estimated duration in seconds
    /// </summary>
    [Required]
    public int DurationSeconds { get; set; }

    /// <summary>
    /// Safety score (0-100, higher is safer)
    /// </summary>
    [Required]
    public double SafetyScore { get; set; } = 0;

    /// <summary>
    /// Number of unsafe zones avoided
    /// </summary>
    public int UnsafeZonesAvoided { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Whether this route is currently active
    /// </summary>
    public bool IsActive { get; set; } = false;

    public DateTime? CompletedAt { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    // Navigation property
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}

