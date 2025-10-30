using SafeSphere.API.Models;

namespace SafeSphere.API.Repositories;

/// <summary>
/// Repository interface for SafeRoute and UnsafeZone operations
/// </summary>
public interface IRouteRepository
{
    // SafeRoute operations
    Task<SafeRoute?> GetSafeRouteByIdAsync(int id);
    Task<SafeRoute?> CreateSafeRouteAsync(SafeRoute route);
    Task<IEnumerable<SafeRoute>> GetUserRoutesAsync(int userId);
    Task<IEnumerable<SafeRoute>> GetActiveRoutesAsync(int userId);
    Task<SafeRoute?> UpdateSafeRouteAsync(SafeRoute route);
    Task<bool> DeleteSafeRouteAsync(int id);
    Task<bool> CompleteRouteAsync(int id);

    // UnsafeZone operations
    Task<UnsafeZone?> GetUnsafeZoneByIdAsync(int id);
    Task<UnsafeZone?> CreateUnsafeZoneAsync(UnsafeZone zone);
    Task<IEnumerable<UnsafeZone>> GetAllUnsafeZonesAsync();
    Task<IEnumerable<UnsafeZone>> GetActiveUnsafeZonesAsync();
    Task<IEnumerable<UnsafeZone>> GetUnsafeZonesByLocationAsync(double latitude, double longitude, double radiusKm);
    Task<UnsafeZone?> UpdateUnsafeZoneAsync(UnsafeZone zone);
    Task<bool> DeleteUnsafeZoneAsync(int id);
    Task<bool> IncrementConfirmationCountAsync(int zoneId);
    Task<IEnumerable<UnsafeZone>> GetExpiredZonesAsync();
    Task<bool> UpdateZoneStatusAsync(int zoneId, string status);
}

