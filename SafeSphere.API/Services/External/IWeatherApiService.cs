namespace SafeSphere.API.Services.External;

/// <summary>
/// Interface for external weather API integration
/// </summary>
public interface IWeatherApiService
{
    /// <summary>
    /// Gets current weather for a specific location
    /// </summary>
    Task<WeatherApiResponse?> GetCurrentWeatherAsync(double latitude, double longitude);

    /// <summary>
    /// Gets weather forecast for a specific location
    /// </summary>
    Task<WeatherForecastResponse?> GetWeatherForecastAsync(double latitude, double longitude, int days = 5);

    /// <summary>
    /// Gets weather alerts for a specific location
    /// </summary>
    Task<List<WeatherApiAlert>?> GetWeatherAlertsAsync(double latitude, double longitude);
}

/// <summary>
/// Response model for weather API
/// </summary>
public class WeatherApiResponse
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
    public List<WeatherApiAlert> Alerts { get; set; } = new();
}

/// <summary>
/// Weather forecast response
/// </summary>
public class WeatherForecastResponse
{
    public string LocationName { get; set; } = string.Empty;
    public List<WeatherForecastDay> Forecast { get; set; } = new();
}

/// <summary>
/// Daily weather forecast
/// </summary>
public class WeatherForecastDay
{
    public DateTime Date { get; set; }
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double WindSpeed { get; set; }
    public int Humidity { get; set; }
}

/// <summary>
/// Weather alert from external API
/// </summary>
public class WeatherApiAlert
{
    public string? ExternalId { get; set; }
    public string Event { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Source { get; set; }
}

