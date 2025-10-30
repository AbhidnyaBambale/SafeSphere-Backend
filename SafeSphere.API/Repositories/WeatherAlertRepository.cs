using Microsoft.EntityFrameworkCore;
using SafeSphere.API.Data;
using SafeSphere.API.Models;

namespace SafeSphere.API.Repositories;

/// <summary>
/// Repository implementation for WeatherAlert and DisasterAlert operations
/// </summary>
public class WeatherAlertRepository : IWeatherAlertRepository
{
    private readonly SafeSphereDbContext _context;
    private readonly ILogger<WeatherAlertRepository> _logger;

    public WeatherAlertRepository(SafeSphereDbContext context, ILogger<WeatherAlertRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region WeatherAlert Operations

    public async Task<WeatherAlert?> GetWeatherAlertByIdAsync(int id)
    {
        try
        {
            return await _context.WeatherAlerts.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather alert by ID: {AlertId}", id);
            return null;
        }
    }

    public async Task<WeatherAlert?> CreateWeatherAlertAsync(WeatherAlert alert)
    {
        try
        {
            await _context.WeatherAlerts.AddAsync(alert);
            await _context.SaveChangesAsync();
            return alert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating weather alert");
            return null;
        }
    }

    public async Task<IEnumerable<WeatherAlert>> GetAllWeatherAlertsAsync()
    {
        try
        {
            return await _context.WeatherAlerts
                .OrderByDescending(a => a.IssuedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all weather alerts");
            return Enumerable.Empty<WeatherAlert>();
        }
    }

    public async Task<IEnumerable<WeatherAlert>> GetActiveWeatherAlertsAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.WeatherAlerts
                .Where(a => a.IsActive && (a.ExpiresAt == null || a.ExpiresAt > now))
                .OrderByDescending(a => a.IssuedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active weather alerts");
            return Enumerable.Empty<WeatherAlert>();
        }
    }

    public async Task<IEnumerable<WeatherAlert>> GetWeatherAlertsByLocationAsync(double latitude, double longitude, double radiusKm)
    {
        try
        {
            var latRange = radiusKm / 111.0;
            var lngRange = radiusKm / (111.0 * Math.Cos(latitude * Math.PI / 180.0));

            var minLat = latitude - latRange;
            var maxLat = latitude + latRange;
            var minLng = longitude - lngRange;
            var maxLng = longitude + lngRange;

            var now = DateTime.UtcNow;
            var alerts = await _context.WeatherAlerts
                .Where(a => a.IsActive &&
                           (a.ExpiresAt == null || a.ExpiresAt > now) &&
                           a.Latitude >= minLat && a.Latitude <= maxLat &&
                           a.Longitude >= minLng && a.Longitude <= maxLng)
                .ToListAsync();

            return alerts.Where(a => CalculateDistance(latitude, longitude, a.Latitude, a.Longitude) <= radiusKm * 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather alerts by location");
            return Enumerable.Empty<WeatherAlert>();
        }
    }

    public async Task<WeatherAlert?> GetWeatherAlertByExternalIdAsync(string externalId)
    {
        try
        {
            return await _context.WeatherAlerts
                .FirstOrDefaultAsync(a => a.ExternalAlertId == externalId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather alert by external ID: {ExternalId}", externalId);
            return null;
        }
    }

    public async Task<WeatherAlert?> UpdateWeatherAlertAsync(WeatherAlert alert)
    {
        try
        {
            _context.WeatherAlerts.Update(alert);
            await _context.SaveChangesAsync();
            return alert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating weather alert: {AlertId}", alert.Id);
            return null;
        }
    }

    public async Task<bool> DeleteWeatherAlertAsync(int id)
    {
        try
        {
            var alert = await _context.WeatherAlerts.FindAsync(id);
            if (alert == null) return false;

            _context.WeatherAlerts.Remove(alert);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting weather alert: {AlertId}", id);
            return false;
        }
    }

    public async Task<bool> DeactivateExpiredAlertsAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var expiredAlerts = await _context.WeatherAlerts
                .Where(a => a.IsActive && a.ExpiresAt != null && a.ExpiresAt <= now)
                .ToListAsync();

            foreach (var alert in expiredAlerts)
            {
                alert.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating expired weather alerts");
            return false;
        }
    }

    #endregion

    #region DisasterAlert Operations

    public async Task<DisasterAlert?> GetDisasterAlertByIdAsync(int id)
    {
        try
        {
            return await _context.DisasterAlerts.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster alert by ID: {AlertId}", id);
            return null;
        }
    }

    public async Task<DisasterAlert?> CreateDisasterAlertAsync(DisasterAlert alert)
    {
        try
        {
            await _context.DisasterAlerts.AddAsync(alert);
            await _context.SaveChangesAsync();
            return alert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating disaster alert");
            return null;
        }
    }

    public async Task<IEnumerable<DisasterAlert>> GetAllDisasterAlertsAsync()
    {
        try
        {
            return await _context.DisasterAlerts
                .OrderByDescending(a => a.IssuedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all disaster alerts");
            return Enumerable.Empty<DisasterAlert>();
        }
    }

    public async Task<IEnumerable<DisasterAlert>> GetActiveDisasterAlertsAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.DisasterAlerts
                .Where(a => a.Status == "Active" && (a.ExpiresAt == null || a.ExpiresAt > now))
                .OrderByDescending(a => a.IssuedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active disaster alerts");
            return Enumerable.Empty<DisasterAlert>();
        }
    }

    public async Task<IEnumerable<DisasterAlert>> GetDisasterAlertsByLocationAsync(double latitude, double longitude, double radiusKm)
    {
        try
        {
            var latRange = radiusKm / 111.0;
            var lngRange = radiusKm / (111.0 * Math.Cos(latitude * Math.PI / 180.0));

            var minLat = latitude - latRange;
            var maxLat = latitude + latRange;
            var minLng = longitude - lngRange;
            var maxLng = longitude + lngRange;

            var now = DateTime.UtcNow;
            var alerts = await _context.DisasterAlerts
                .Where(a => a.Status == "Active" &&
                           (a.ExpiresAt == null || a.ExpiresAt > now) &&
                           a.Latitude >= minLat && a.Latitude <= maxLat &&
                           a.Longitude >= minLng && a.Longitude <= maxLng)
                .ToListAsync();

            return alerts.Where(a => CalculateDistance(latitude, longitude, a.Latitude, a.Longitude) <= radiusKm * 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster alerts by location");
            return Enumerable.Empty<DisasterAlert>();
        }
    }

    public async Task<IEnumerable<DisasterAlert>> GetDisasterAlertsByTypeAsync(string disasterType)
    {
        try
        {
            return await _context.DisasterAlerts
                .Where(a => a.DisasterType == disasterType && a.Status == "Active")
                .OrderByDescending(a => a.IssuedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster alerts by type: {Type}", disasterType);
            return Enumerable.Empty<DisasterAlert>();
        }
    }

    public async Task<DisasterAlert?> GetDisasterAlertByExternalIdAsync(string externalId)
    {
        try
        {
            return await _context.DisasterAlerts
                .FirstOrDefaultAsync(a => a.ExternalAlertId == externalId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster alert by external ID: {ExternalId}", externalId);
            return null;
        }
    }

    public async Task<DisasterAlert?> UpdateDisasterAlertAsync(DisasterAlert alert)
    {
        try
        {
            alert.UpdatedAt = DateTime.UtcNow;
            _context.DisasterAlerts.Update(alert);
            await _context.SaveChangesAsync();
            return alert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating disaster alert: {AlertId}", alert.Id);
            return null;
        }
    }

    public async Task<bool> DeleteDisasterAlertAsync(int id)
    {
        try
        {
            var alert = await _context.DisasterAlerts.FindAsync(id);
            if (alert == null) return false;

            _context.DisasterAlerts.Remove(alert);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting disaster alert: {AlertId}", id);
            return false;
        }
    }

    public async Task<bool> IncrementDisasterConfirmationAsync(int alertId)
    {
        try
        {
            var alert = await _context.DisasterAlerts.FindAsync(alertId);
            if (alert == null) return false;

            alert.ConfirmationCount++;
            alert.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing disaster confirmation: {AlertId}", alertId);
            return false;
        }
    }

    public async Task<bool> UpdateDisasterStatusAsync(int alertId, string status)
    {
        try
        {
            var alert = await _context.DisasterAlerts.FindAsync(alertId);
            if (alert == null) return false;

            alert.Status = status;
            alert.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating disaster status: {AlertId}", alertId);
            return false;
        }
    }

    #endregion

    #region Helper Methods

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000; // Earth's radius in meters

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c; // Distance in meters
    }

    private double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    #endregion
}

