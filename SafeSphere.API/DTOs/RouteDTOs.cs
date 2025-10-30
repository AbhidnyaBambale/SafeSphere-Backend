using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.DTOs;

// Request DTOs

/// <summary>
/// DTO for requesting a safe route between two locations
/// </summary>
public class GetSafeRouteRequestDto
{
    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double OriginLat { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double OriginLng { get; set; }

    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double DestinationLat { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double DestinationLng { get; set; }

    /// <summary>
    /// Optional: preferred transport mode (driving, walking, cycling)
    /// </summary>
    [MaxLength(20)]
    public string? TransportMode { get; set; } = "driving";

    /// <summary>
    /// Optional: whether to avoid highways
    /// </summary>
    public bool AvoidHighways { get; set; } = false;
}

/// <summary>
/// DTO for creating a safe route record
/// </summary>
public class CreateSafeRouteDto
{
    [Required]
    public double OriginLat { get; set; }

    [Required]
    public double OriginLng { get; set; }

    [Required]
    public double DestinationLat { get; set; }

    [Required]
    public double DestinationLng { get; set; }

    [Required]
    public string RouteCoordinates { get; set; } = "[]";

    [Required]
    public double DistanceMeters { get; set; }

    [Required]
    public int DurationSeconds { get; set; }

    public double SafetyScore { get; set; } = 0;

    public int UnsafeZonesAvoided { get; set; } = 0;

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

// Response DTOs

/// <summary>
/// DTO for route coordinate point
/// </summary>
public class RoutePointDto
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

/// <summary>
/// DTO for safe route response
/// </summary>
public class SafeRouteResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double OriginLat { get; set; }
    public double OriginLng { get; set; }
    public double DestinationLat { get; set; }
    public double DestinationLng { get; set; }
    public List<RoutePointDto> RouteCoordinates { get; set; } = new();
    public double DistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public double SafetyScore { get; set; }
    public int UnsafeZonesAvoided { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public List<UnsafeZoneResponseDto> NearbyUnsafeZones { get; set; } = new();
}

/// <summary>
/// DTO for unsafe zone creation
/// </summary>
public class CreateUnsafeZoneDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(-90, 90)]
    public double CenterLat { get; set; }

    [Required]
    [Range(-180, 180)]
    public double CenterLng { get; set; }

    [Range(50, 10000, ErrorMessage = "Radius must be between 50 and 10000 meters")]
    public double RadiusMeters { get; set; } = 500;

    [MaxLength(50)]
    public string Severity { get; set; } = "Medium"; // Low, Medium, High, Critical

    [MaxLength(50)]
    public string ThreatType { get; set; } = "Other"; // Crime, Accident, Natural, Construction, Other

    public int? ExpiresInHours { get; set; } // null for permanent zones

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// DTO for updating unsafe zone
/// </summary>
public class UpdateUnsafeZoneDto
{
    [MaxLength(50)]
    public string? Status { get; set; } // Active, Resolved, Expired

    [Range(0, int.MaxValue)]
    public int? ConfirmationCount { get; set; }

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// DTO for unsafe zone response
/// </summary>
public class UnsafeZoneResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double CenterLat { get; set; }
    public double CenterLng { get; set; }
    public double RadiusMeters { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string ThreatType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? ReportedByUserId { get; set; }
    public int ConfirmationCount { get; set; }
    public string? AdditionalInfo { get; set; }
    public double? DistanceFromUser { get; set; } // In meters, calculated if user location provided
}

/// <summary>
/// DTO for confirming an unsafe zone
/// </summary>
public class ConfirmUnsafeZoneDto
{
    [Required]
    public int ZoneId { get; set; }

    [MaxLength(500)]
    public string? AdditionalComment { get; set; }
}

