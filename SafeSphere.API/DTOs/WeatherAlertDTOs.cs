using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.DTOs;

// Request DTOs

/// <summary>
/// DTO for requesting weather alerts for a location
/// </summary>
public class GetWeatherAlertsRequestDto
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
    [Range(1, 500, ErrorMessage = "Radius must be between 1 and 500 km")]
    public double RadiusKm { get; set; } = 50;

    /// <summary>
    /// Optional: filter by severity (Info, Warning, Severe, Extreme)
    /// </summary>
    [MaxLength(50)]
    public string? MinimumSeverity { get; set; }

    /// <summary>
    /// Optional: only return active alerts
    /// </summary>
    public bool ActiveOnly { get; set; } = true;
}

/// <summary>
/// DTO for creating a weather alert
/// </summary>
public class CreateWeatherAlertDto
{
    [Required]
    [MaxLength(100)]
    public string LocationName { get; set; } = string.Empty;

    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Required]
    [MaxLength(50)]
    public string WeatherCondition { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public double? Temperature { get; set; }

    [MaxLength(50)]
    public string Severity { get; set; } = "Info";

    [MaxLength(100)]
    public string? ExternalAlertId { get; set; }

    public DateTime? ExpiresAt { get; set; }

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }

    [MaxLength(50)]
    public string? DataSource { get; set; }
}

/// <summary>
/// DTO for weather alert response
/// </summary>
public class WeatherAlertResponseDto
{
    public int Id { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string WeatherCondition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double? Temperature { get; set; }
    public string Severity { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? DataSource { get; set; }
    public double? DistanceKm { get; set; } // Distance from requested location
    public int? MinutesUntilExpiry { get; set; }
}

/// <summary>
/// DTO for current weather information
/// </summary>
public class CurrentWeatherDto
{
    public string LocationName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int? Visibility { get; set; }
    public DateTime Timestamp { get; set; }
    public List<WeatherAlertResponseDto> Alerts { get; set; } = new();
}

