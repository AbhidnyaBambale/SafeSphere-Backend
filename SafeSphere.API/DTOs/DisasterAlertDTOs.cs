using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.DTOs;

// Request DTOs

/// <summary>
/// DTO for requesting disaster alerts for a location
/// </summary>
public class GetDisasterAlertsRequestDto
{
    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Latitude { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Longitude { get; set; }

    /// <summary>
    /// Optional: radius in kilometers to search for alerts
    /// </summary>
    [Range(1, 1000, ErrorMessage = "Radius must be between 1 and 1000 km")]
    public double RadiusKm { get; set; } = 100;

    /// <summary>
    /// Optional: filter by disaster type
    /// </summary>
    [MaxLength(50)]
    public string? DisasterType { get; set; }

    /// <summary>
    /// Optional: filter by minimum severity
    /// </summary>
    [MaxLength(50)]
    public string? MinimumSeverity { get; set; }

    /// <summary>
    /// Optional: only return active alerts
    /// </summary>
    public bool ActiveOnly { get; set; } = true;
}

/// <summary>
/// DTO for creating a disaster alert
/// </summary>
public class CreateDisasterAlertDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DisasterType { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string AffectedArea { get; set; } = string.Empty;

    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Range(0, 10000)]
    public double? AffectedRadiusKm { get; set; }

    [MaxLength(50)]
    public string Severity { get; set; } = "Moderate";

    [MaxLength(100)]
    public string? ExternalAlertId { get; set; }

    [MaxLength(100)]
    public string? Source { get; set; }

    public DateTime? ExpiresAt { get; set; }

    [MaxLength(2000)]
    public string? SafetyInstructions { get; set; }

    [MaxLength(500)]
    public string? EmergencyContactInfo { get; set; }
}

/// <summary>
/// DTO for updating a disaster alert
/// </summary>
public class UpdateDisasterAlertDto
{
    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; } // Active, Resolved, Monitoring

    [MaxLength(50)]
    public string? Severity { get; set; }

    public DateTime? ExpiresAt { get; set; }

    [MaxLength(2000)]
    public string? SafetyInstructions { get; set; }
}

/// <summary>
/// DTO for disaster alert response
/// </summary>
public class DisasterAlertResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DisasterType { get; set; } = string.Empty;
    public string AffectedArea { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? AffectedRadiusKm { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? Source { get; set; }
    public int ConfirmationCount { get; set; }
    public string? SafetyInstructions { get; set; }
    public string? EmergencyContactInfo { get; set; }
    public double? DistanceKm { get; set; } // Distance from requested location
    public bool IsUserInAffectedArea { get; set; }
}

/// <summary>
/// DTO for confirming a disaster alert
/// </summary>
public class ConfirmDisasterAlertDto
{
    [Required]
    public int AlertId { get; set; }

    [MaxLength(500)]
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// DTO for disaster statistics
/// </summary>
public class DisasterStatisticsDto
{
    public int TotalActiveAlerts { get; set; }
    public int CriticalAlerts { get; set; }
    public Dictionary<string, int> AlertsByType { get; set; } = new();
    public Dictionary<string, int> AlertsBySeverity { get; set; } = new();
    public List<DisasterAlertResponseDto> RecentAlerts { get; set; } = new();
}

