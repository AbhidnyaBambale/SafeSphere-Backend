using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.Models;

/// <summary>
/// Represents a weather alert for a specific location
/// </summary>
public class WeatherAlert
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string LocationName { get; set; } = string.Empty;

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    /// <summary>
    /// Weather condition: Clear, Rain, Snow, Storm, Fog, Extreme, etc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string WeatherCondition { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Temperature in Celsius
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Alert severity: Info, Warning, Severe, Extreme
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Severity { get; set; } = "Info";

    /// <summary>
    /// External API alert ID (for deduplication)
    /// </summary>
    [MaxLength(100)]
    public string? ExternalAlertId { get; set; }

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Whether this alert is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    [MaxLength(1000)]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Source of the weather data: OpenWeatherMap, NOAA, etc.
    /// </summary>
    [MaxLength(50)]
    public string? DataSource { get; set; }
}

