using Microsoft.EntityFrameworkCore;
using SafeSphere.API.Data;
using SafeSphere.API.Models;

namespace SafeSphere.API.Repositories;

/// <summary>
/// Repository implementation for SafeRoute and UnsafeZone operations
/// </summary>
public class RouteRepository : IRouteRepository
{
    private readonly SafeSphereDbContext _context;
    private readonly ILogger<RouteRepository> _logger;

    public RouteRepository(SafeSphereDbContext context, ILogger<RouteRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region SafeRoute Operations

    public async Task<SafeRoute?> GetSafeRouteByIdAsync(int id)
    {
        try
        {
            return await _context.SafeRoutes
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting safe route by ID: {RouteId}", id);
            return null;
        }
    }

    public async Task<SafeRoute?> CreateSafeRouteAsync(SafeRoute route)
    {
        try
        {
            await _context.SafeRoutes.AddAsync(route);
            await _context.SaveChangesAsync();
            return route;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating safe route");
            return null;
        }
    }

    public async Task<IEnumerable<SafeRoute>> GetUserRoutesAsync(int userId)
    {
        try
        {
            return await _context.SafeRoutes
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user routes for user: {UserId}", userId);
            return Enumerable.Empty<SafeRoute>();
        }
    }

    public async Task<IEnumerable<SafeRoute>> GetActiveRoutesAsync(int userId)
    {
        try
        {
            return await _context.SafeRoutes
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active routes for user: {UserId}", userId);
            return Enumerable.Empty<SafeRoute>();
        }
    }

    public async Task<SafeRoute?> UpdateSafeRouteAsync(SafeRoute route)
    {
        try
        {
            _context.SafeRoutes.Update(route);
            await _context.SaveChangesAsync();
            return route;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating safe route: {RouteId}", route.Id);
            return null;
        }
    }

    public async Task<bool> DeleteSafeRouteAsync(int id)
    {
        try
        {
            var route = await _context.SafeRoutes.FindAsync(id);
            if (route == null) return false;

            _context.SafeRoutes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting safe route: {RouteId}", id);
            return false;
        }
    }

    public async Task<bool> CompleteRouteAsync(int id)
    {
        try
        {
            var route = await _context.SafeRoutes.FindAsync(id);
            if (route == null) return false;

            route.IsActive = false;
            route.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing route: {RouteId}", id);
            return false;
        }
    }

    #endregion

    #region UnsafeZone Operations

    public async Task<UnsafeZone?> GetUnsafeZoneByIdAsync(int id)
    {
        try
        {
            return await _context.UnsafeZones.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unsafe zone by ID: {ZoneId}", id);
            return null;
        }
    }

    public async Task<UnsafeZone?> CreateUnsafeZoneAsync(UnsafeZone zone)
    {
        try
        {
            await _context.UnsafeZones.AddAsync(zone);
            await _context.SaveChangesAsync();
            return zone;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating unsafe zone");
            return null;
        }
    }

    public async Task<IEnumerable<UnsafeZone>> GetAllUnsafeZonesAsync()
    {
        try
        {
            return await _context.UnsafeZones
                .OrderByDescending(z => z.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all unsafe zones");
            return Enumerable.Empty<UnsafeZone>();
        }
    }

    public async Task<IEnumerable<UnsafeZone>> GetActiveUnsafeZonesAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.UnsafeZones
                .Where(z => z.Status == "Active" && (z.ExpiresAt == null || z.ExpiresAt > now))
                .OrderByDescending(z => z.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active unsafe zones");
            return Enumerable.Empty<UnsafeZone>();
        }
    }

    public async Task<IEnumerable<UnsafeZone>> GetUnsafeZonesByLocationAsync(double latitude, double longitude, double radiusKm)
    {
        try
        {
            // Simple bounding box calculation for performance
            // For more accurate results, could use PostGIS or more sophisticated geospatial queries
            var latRange = radiusKm / 111.0; // Approximate km per degree latitude
            var lngRange = radiusKm / (111.0 * Math.Cos(latitude * Math.PI / 180.0));

            var minLat = latitude - latRange;
            var maxLat = latitude + latRange;
            var minLng = longitude - lngRange;
            var maxLng = longitude + lngRange;

            var zones = await _context.UnsafeZones
                .Where(z => z.Status == "Active" &&
                           z.CenterLat >= minLat && z.CenterLat <= maxLat &&
                           z.CenterLng >= minLng && z.CenterLng <= maxLng)
                .ToListAsync();

            // Filter by actual distance using Haversine formula
            return zones.Where(z => CalculateDistance(latitude, longitude, z.CenterLat, z.CenterLng) <= radiusKm * 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unsafe zones by location");
            return Enumerable.Empty<UnsafeZone>();
        }
    }

    public async Task<UnsafeZone?> UpdateUnsafeZoneAsync(UnsafeZone zone)
    {
        try
        {
            _context.UnsafeZones.Update(zone);
            await _context.SaveChangesAsync();
            return zone;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unsafe zone: {ZoneId}", zone.Id);
            return null;
        }
    }

    public async Task<bool> DeleteUnsafeZoneAsync(int id)
    {
        try
        {
            var zone = await _context.UnsafeZones.FindAsync(id);
            if (zone == null) return false;

            _context.UnsafeZones.Remove(zone);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting unsafe zone: {ZoneId}", id);
            return false;
        }
    }

    public async Task<bool> IncrementConfirmationCountAsync(int zoneId)
    {
        try
        {
            var zone = await _context.UnsafeZones.FindAsync(zoneId);
            if (zone == null) return false;

            zone.ConfirmationCount++;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing confirmation count for zone: {ZoneId}", zoneId);
            return false;
        }
    }

    public async Task<IEnumerable<UnsafeZone>> GetExpiredZonesAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.UnsafeZones
                .Where(z => z.ExpiresAt != null && z.ExpiresAt <= now && z.Status == "Active")
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting expired zones");
            return Enumerable.Empty<UnsafeZone>();
        }
    }

    public async Task<bool> UpdateZoneStatusAsync(int zoneId, string status)
    {
        try
        {
            var zone = await _context.UnsafeZones.FindAsync(zoneId);
            if (zone == null) return false;

            zone.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating zone status: {ZoneId}", zoneId);
            return false;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Calculate distance between two points using Haversine formula
    /// </summary>
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

