using SafeSphere.API.Models;

namespace SafeSphere.API.Repositories;

/// <summary>
/// Repository interface for WeatherAlert and DisasterAlert operations
/// </summary>
public interface IWeatherAlertRepository
{
    // WeatherAlert operations
    Task<WeatherAlert?> GetWeatherAlertByIdAsync(int id);
    Task<WeatherAlert?> CreateWeatherAlertAsync(WeatherAlert alert);
    Task<IEnumerable<WeatherAlert>> GetAllWeatherAlertsAsync();
    Task<IEnumerable<WeatherAlert>> GetActiveWeatherAlertsAsync();
    Task<IEnumerable<WeatherAlert>> GetWeatherAlertsByLocationAsync(double latitude, double longitude, double radiusKm);
    Task<WeatherAlert?> GetWeatherAlertByExternalIdAsync(string externalId);
    Task<WeatherAlert?> UpdateWeatherAlertAsync(WeatherAlert alert);
    Task<bool> DeleteWeatherAlertAsync(int id);
    Task<bool> DeactivateExpiredAlertsAsync();

    // DisasterAlert operations
    Task<DisasterAlert?> GetDisasterAlertByIdAsync(int id);
    Task<DisasterAlert?> CreateDisasterAlertAsync(DisasterAlert alert);
    Task<IEnumerable<DisasterAlert>> GetAllDisasterAlertsAsync();
    Task<IEnumerable<DisasterAlert>> GetActiveDisasterAlertsAsync();
    Task<IEnumerable<DisasterAlert>> GetDisasterAlertsByLocationAsync(double latitude, double longitude, double radiusKm);
    Task<IEnumerable<DisasterAlert>> GetDisasterAlertsByTypeAsync(string disasterType);
    Task<DisasterAlert?> GetDisasterAlertByExternalIdAsync(string externalId);
    Task<DisasterAlert?> UpdateDisasterAlertAsync(DisasterAlert alert);
    Task<bool> DeleteDisasterAlertAsync(int id);
    Task<bool> IncrementDisasterConfirmationAsync(int alertId);
    Task<bool> UpdateDisasterStatusAsync(int alertId, string status);
}

