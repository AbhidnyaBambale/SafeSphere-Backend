using SafeSphere.API.DTOs;

namespace SafeSphere.API.Services;

/// <summary>
/// Service interface for safe route and unsafe zone operations
/// </summary>
public interface ISafeRouteService
{
    // SafeRoute operations
    Task<SafeRouteResponseDto?> GetSafeRouteAsync(int userId, GetSafeRouteRequestDto request);
    Task<SafeRouteResponseDto?> GetRouteByIdAsync(int id);
    Task<IEnumerable<SafeRouteResponseDto>> GetUserRoutesAsync(int userId);
    Task<IEnumerable<SafeRouteResponseDto>> GetActiveUserRoutesAsync(int userId);
    Task<bool> CompleteRouteAsync(int routeId);
    Task<bool> DeleteRouteAsync(int routeId);

    // UnsafeZone operations
    Task<UnsafeZoneResponseDto?> CreateUnsafeZoneAsync(int userId, CreateUnsafeZoneDto dto);
    Task<UnsafeZoneResponseDto?> GetUnsafeZoneByIdAsync(int id);
    Task<IEnumerable<UnsafeZoneResponseDto>> GetAllUnsafeZonesAsync();
    Task<IEnumerable<UnsafeZoneResponseDto>> GetActiveUnsafeZonesAsync();
    Task<IEnumerable<UnsafeZoneResponseDto>> GetUnsafeZonesByLocationAsync(double latitude, double longitude, double radiusKm);
    Task<UnsafeZoneResponseDto?> UpdateUnsafeZoneAsync(int zoneId, UpdateUnsafeZoneDto dto);
    Task<bool> DeleteUnsafeZoneAsync(int zoneId);
    Task<bool> ConfirmUnsafeZoneAsync(int zoneId, int userId);
}

