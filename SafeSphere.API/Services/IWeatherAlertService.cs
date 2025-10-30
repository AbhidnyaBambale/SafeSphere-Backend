using SafeSphere.API.DTOs;

namespace SafeSphere.API.Services;

/// <summary>
/// Service interface for weather alert operations
/// </summary>
public interface IWeatherAlertService
{
    // Weather operations
    Task<CurrentWeatherDto?> GetCurrentWeatherAsync(double latitude, double longitude);
    Task<IEnumerable<WeatherAlertResponseDto>> GetWeatherAlertsAsync(GetWeatherAlertsRequestDto request);
    Task<WeatherAlertResponseDto?> CreateWeatherAlertAsync(CreateWeatherAlertDto dto);
    Task<WeatherAlertResponseDto?> GetWeatherAlertByIdAsync(int id);
    Task<IEnumerable<WeatherAlertResponseDto>> GetAllActiveWeatherAlertsAsync();
    Task<bool> DeleteWeatherAlertAsync(int id);
    Task SyncWeatherAlertsAsync(); // Background job to sync from external API
}

