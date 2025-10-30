using AutoMapper;
using SafeSphere.API.DTOs;
using SafeSphere.API.Models;
using SafeSphere.API.Repositories;
using SafeSphere.API.Services.External;

namespace SafeSphere.API.Services;

/// <summary>
/// Service implementation for weather alert operations
/// </summary>
public class WeatherAlertService : IWeatherAlertService
{
    private readonly IWeatherAlertRepository _repository;
    private readonly IWeatherApiService _weatherApiService;
    private readonly IMapper _mapper;
    private readonly ILogger<WeatherAlertService> _logger;

    public WeatherAlertService(
        IWeatherAlertRepository repository,
        IWeatherApiService weatherApiService,
        IMapper mapper,
        ILogger<WeatherAlertService> logger)
    {
        _repository = repository;
        _weatherApiService = weatherApiService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        try
        {
            var weather = await _weatherApiService.GetCurrentWeatherAsync(latitude, longitude);
            if (weather == null) return null;

            var alerts = await _repository.GetWeatherAlertsByLocationAsync(latitude, longitude, 50);

            var currentWeather = new CurrentWeatherDto
            {
                LocationName = weather.LocationName,
                Latitude = weather.Latitude,
                Longitude = weather.Longitude,
                Condition = weather.Condition,
                Description = weather.Description,
                Temperature = weather.Temperature,
                FeelsLike = weather.FeelsLike,
                Humidity = weather.Humidity,
                WindSpeed = weather.WindSpeed,
                Visibility = weather.Visibility ?? 0,
                Timestamp = weather.Timestamp,
                Alerts = _mapper.Map<List<WeatherAlertResponseDto>>(alerts)
            };

            return currentWeather;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current weather");
            return null;
        }
    }

    public async Task<IEnumerable<WeatherAlertResponseDto>> GetWeatherAlertsAsync(GetWeatherAlertsRequestDto request)
    {
        try
        {
            var alerts = await _repository.GetWeatherAlertsByLocationAsync(
                request.Latitude, request.Longitude, request.RadiusKm);

            var response = _mapper.Map<List<WeatherAlertResponseDto>>(alerts);

            // Calculate distance and time until expiry
            foreach (var alert in response)
            {
                alert.DistanceKm = CalculateDistance(request.Latitude, request.Longitude, 
                                                    alert.Latitude, alert.Longitude) / 1000;
                
                if (alert.ExpiresAt.HasValue)
                {
                    var minutesUntil = (alert.ExpiresAt.Value - DateTime.UtcNow).TotalMinutes;
                    alert.MinutesUntilExpiry = (int)Math.Max(0, minutesUntil);
                }
            }

            // Filter by severity if specified
            if (!string.IsNullOrEmpty(request.MinimumSeverity))
            {
                response = FilterBySeverity(response, request.MinimumSeverity);
            }

            return response.OrderBy(a => a.DistanceKm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather alerts");
            return Enumerable.Empty<WeatherAlertResponseDto>();
        }
    }

    public async Task<WeatherAlertResponseDto?> CreateWeatherAlertAsync(CreateWeatherAlertDto dto)
    {
        try
        {
            var alert = new WeatherAlert
            {
                LocationName = dto.LocationName,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                WeatherCondition = dto.WeatherCondition,
                Description = dto.Description,
                Temperature = dto.Temperature,
                Severity = dto.Severity,
                ExternalAlertId = dto.ExternalAlertId,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = dto.ExpiresAt,
                IsActive = true,
                AdditionalInfo = dto.AdditionalInfo,
                DataSource = dto.DataSource
            };

            var created = await _repository.CreateWeatherAlertAsync(alert);
            return created != null ? _mapper.Map<WeatherAlertResponseDto>(created) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating weather alert");
            return null;
        }
    }

    public async Task<WeatherAlertResponseDto?> GetWeatherAlertByIdAsync(int id)
    {
        try
        {
            var alert = await _repository.GetWeatherAlertByIdAsync(id);
            return alert != null ? _mapper.Map<WeatherAlertResponseDto>(alert) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather alert by ID: {AlertId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<WeatherAlertResponseDto>> GetAllActiveWeatherAlertsAsync()
    {
        try
        {
            var alerts = await _repository.GetActiveWeatherAlertsAsync();
            return _mapper.Map<IEnumerable<WeatherAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all active weather alerts");
            return Enumerable.Empty<WeatherAlertResponseDto>();
        }
    }

    public async Task<bool> DeleteWeatherAlertAsync(int id)
    {
        try
        {
            return await _repository.DeleteWeatherAlertAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting weather alert: {AlertId}", id);
            return false;
        }
    }

    public async Task SyncWeatherAlertsAsync()
    {
        try
        {
            _logger.LogInformation("Starting weather alerts sync");

            // Deactivate expired alerts
            await _repository.DeactivateExpiredAlertsAsync();

            // In production, you would:
            // 1. Fetch alerts from external APIs for major cities/regions
            // 2. Check for duplicates using ExternalAlertId
            // 3. Create or update alerts in database
            
            _logger.LogInformation("Weather alerts sync completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing weather alerts");
        }
    }

    #region Helper Methods

    private List<WeatherAlertResponseDto> FilterBySeverity(List<WeatherAlertResponseDto> alerts, string minimumSeverity)
    {
        var severityOrder = new Dictionary<string, int>
        {
            { "Info", 1 },
            { "Warning", 2 },
            { "Severe", 3 },
            { "Extreme", 4 }
        };

        var minLevel = severityOrder.GetValueOrDefault(minimumSeverity, 1);
        return alerts.Where(a => severityOrder.GetValueOrDefault(a.Severity, 1) >= minLevel).ToList();
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000;
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private double ToRadians(double degrees) => degrees * Math.PI / 180.0;

    #endregion
}

