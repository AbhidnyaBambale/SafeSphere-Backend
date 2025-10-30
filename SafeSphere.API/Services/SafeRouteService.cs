using AutoMapper;
using SafeSphere.API.DTOs;
using SafeSphere.API.Models;
using SafeSphere.API.Repositories;
using System.Text.Json;

namespace SafeSphere.API.Services;

/// <summary>
/// Service implementation for safe route and unsafe zone operations
/// </summary>
public class SafeRouteService : ISafeRouteService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SafeRouteService> _logger;

    public SafeRouteService(
        IRouteRepository routeRepository,
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<SafeRouteService> logger)
    {
        _routeRepository = routeRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    #region SafeRoute Operations

    public async Task<SafeRouteResponseDto?> GetSafeRouteAsync(int userId, GetSafeRouteRequestDto request)
    {
        try
        {
            // Verify user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return null;
            }

            // Get nearby unsafe zones
            var unsafeZones = await _routeRepository.GetUnsafeZonesByLocationAsync(
                request.OriginLat, request.OriginLng, 10); // 10km radius

            // Calculate safe route (simplified version - in production, integrate with Google Maps Directions API)
            var route = await CalculateSafeRouteAsync(request, unsafeZones.ToList());

            // Create route record
            var safeRoute = new SafeRoute
            {
                UserId = userId,
                OriginLat = request.OriginLat,
                OriginLng = request.OriginLng,
                DestinationLat = request.DestinationLat,
                DestinationLng = request.DestinationLng,
                RouteCoordinates = JsonSerializer.Serialize(route.Coordinates),
                DistanceMeters = route.DistanceMeters,
                DurationSeconds = route.DurationSeconds,
                SafetyScore = route.SafetyScore,
                UnsafeZonesAvoided = route.UnsafeZonesAvoided,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdRoute = await _routeRepository.CreateSafeRouteAsync(safeRoute);
            if (createdRoute == null) return null;

            var response = _mapper.Map<SafeRouteResponseDto>(createdRoute);
            response.RouteCoordinates = route.Coordinates;
            response.NearbyUnsafeZones = _mapper.Map<List<UnsafeZoneResponseDto>>(route.NearbyZones);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating safe route for user: {UserId}", userId);
            return null;
        }
    }

    public async Task<SafeRouteResponseDto?> GetRouteByIdAsync(int id)
    {
        try
        {
            var route = await _routeRepository.GetSafeRouteByIdAsync(id);
            if (route == null) return null;

            var response = _mapper.Map<SafeRouteResponseDto>(route);
            
            // Deserialize route coordinates
            if (!string.IsNullOrEmpty(route.RouteCoordinates))
            {
                response.RouteCoordinates = JsonSerializer.Deserialize<List<RoutePointDto>>(route.RouteCoordinates) ?? new List<RoutePointDto>();
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting route by ID: {RouteId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<SafeRouteResponseDto>> GetUserRoutesAsync(int userId)
    {
        try
        {
            var routes = await _routeRepository.GetUserRoutesAsync(userId);
            var response = _mapper.Map<IEnumerable<SafeRouteResponseDto>>(routes);

            // Deserialize coordinates for each route
            foreach (var route in response)
            {
                var originalRoute = routes.FirstOrDefault(r => r.Id == route.Id);
                if (originalRoute != null && !string.IsNullOrEmpty(originalRoute.RouteCoordinates))
                {
                    route.RouteCoordinates = JsonSerializer.Deserialize<List<RoutePointDto>>(originalRoute.RouteCoordinates) ?? new List<RoutePointDto>();
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user routes: {UserId}", userId);
            return Enumerable.Empty<SafeRouteResponseDto>();
        }
    }

    public async Task<IEnumerable<SafeRouteResponseDto>> GetActiveUserRoutesAsync(int userId)
    {
        try
        {
            var routes = await _routeRepository.GetActiveRoutesAsync(userId);
            return _mapper.Map<IEnumerable<SafeRouteResponseDto>>(routes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active user routes: {UserId}", userId);
            return Enumerable.Empty<SafeRouteResponseDto>();
        }
    }

    public async Task<bool> CompleteRouteAsync(int routeId)
    {
        try
        {
            return await _routeRepository.CompleteRouteAsync(routeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing route: {RouteId}", routeId);
            return false;
        }
    }

    public async Task<bool> DeleteRouteAsync(int routeId)
    {
        try
        {
            return await _routeRepository.DeleteSafeRouteAsync(routeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting route: {RouteId}", routeId);
            return false;
        }
    }

    #endregion

    #region UnsafeZone Operations

    public async Task<UnsafeZoneResponseDto?> CreateUnsafeZoneAsync(int userId, CreateUnsafeZoneDto dto)
    {
        try
        {
            var zone = new UnsafeZone
            {
                Name = dto.Name,
                Description = dto.Description,
                CenterLat = dto.CenterLat,
                CenterLng = dto.CenterLng,
                RadiusMeters = dto.RadiusMeters,
                Severity = dto.Severity,
                ThreatType = dto.ThreatType,
                Status = "Active",
                ReportedByUserId = userId,
                ConfirmationCount = 1,
                AdditionalInfo = dto.AdditionalInfo,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = dto.ExpiresInHours.HasValue ? DateTime.UtcNow.AddHours(dto.ExpiresInHours.Value) : null
            };

            var createdZone = await _routeRepository.CreateUnsafeZoneAsync(zone);
            return createdZone != null ? _mapper.Map<UnsafeZoneResponseDto>(createdZone) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating unsafe zone");
            return null;
        }
    }

    public async Task<UnsafeZoneResponseDto?> GetUnsafeZoneByIdAsync(int id)
    {
        try
        {
            var zone = await _routeRepository.GetUnsafeZoneByIdAsync(id);
            return zone != null ? _mapper.Map<UnsafeZoneResponseDto>(zone) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unsafe zone by ID: {ZoneId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<UnsafeZoneResponseDto>> GetAllUnsafeZonesAsync()
    {
        try
        {
            var zones = await _routeRepository.GetAllUnsafeZonesAsync();
            return _mapper.Map<IEnumerable<UnsafeZoneResponseDto>>(zones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all unsafe zones");
            return Enumerable.Empty<UnsafeZoneResponseDto>();
        }
    }

    public async Task<IEnumerable<UnsafeZoneResponseDto>> GetActiveUnsafeZonesAsync()
    {
        try
        {
            var zones = await _routeRepository.GetActiveUnsafeZonesAsync();
            return _mapper.Map<IEnumerable<UnsafeZoneResponseDto>>(zones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active unsafe zones");
            return Enumerable.Empty<UnsafeZoneResponseDto>();
        }
    }

    public async Task<IEnumerable<UnsafeZoneResponseDto>> GetUnsafeZonesByLocationAsync(double latitude, double longitude, double radiusKm)
    {
        try
        {
            var zones = await _routeRepository.GetUnsafeZonesByLocationAsync(latitude, longitude, radiusKm);
            var response = _mapper.Map<List<UnsafeZoneResponseDto>>(zones);

            // Calculate distance from user location
            foreach (var zone in response)
            {
                zone.DistanceFromUser = CalculateDistance(latitude, longitude, zone.CenterLat, zone.CenterLng);
            }

            return response.OrderBy(z => z.DistanceFromUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unsafe zones by location");
            return Enumerable.Empty<UnsafeZoneResponseDto>();
        }
    }

    public async Task<UnsafeZoneResponseDto?> UpdateUnsafeZoneAsync(int zoneId, UpdateUnsafeZoneDto dto)
    {
        try
        {
            var zone = await _routeRepository.GetUnsafeZoneByIdAsync(zoneId);
            if (zone == null) return null;

            if (!string.IsNullOrEmpty(dto.Status))
                zone.Status = dto.Status;

            if (dto.ConfirmationCount.HasValue)
                zone.ConfirmationCount = dto.ConfirmationCount.Value;

            if (!string.IsNullOrEmpty(dto.AdditionalInfo))
                zone.AdditionalInfo = dto.AdditionalInfo;

            var updated = await _routeRepository.UpdateUnsafeZoneAsync(zone);
            return updated != null ? _mapper.Map<UnsafeZoneResponseDto>(updated) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unsafe zone: {ZoneId}", zoneId);
            return null;
        }
    }

    public async Task<bool> DeleteUnsafeZoneAsync(int zoneId)
    {
        try
        {
            return await _routeRepository.DeleteUnsafeZoneAsync(zoneId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting unsafe zone: {ZoneId}", zoneId);
            return false;
        }
    }

    public async Task<bool> ConfirmUnsafeZoneAsync(int zoneId, int userId)
    {
        try
        {
            return await _routeRepository.IncrementConfirmationCountAsync(zoneId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming unsafe zone: {ZoneId}", zoneId);
            return false;
        }
    }

    #endregion

    #region Helper Methods

    private async Task<(List<RoutePointDto> Coordinates, double DistanceMeters, int DurationSeconds, double SafetyScore, int UnsafeZonesAvoided, List<UnsafeZone> NearbyZones)> 
        CalculateSafeRouteAsync(GetSafeRouteRequestDto request, List<UnsafeZone> unsafeZones)
    {
        // Simplified route calculation
        // In production, integrate with Google Maps Directions API or OpenRouteService
        
        var coordinates = new List<RoutePointDto>
        {
            new RoutePointDto { Lat = request.OriginLat, Lng = request.OriginLng },
            new RoutePointDto { Lat = request.DestinationLat, Lng = request.DestinationLng }
        };

        var distance = CalculateDistance(request.OriginLat, request.OriginLng, 
                                        request.DestinationLat, request.DestinationLng);

        // Calculate safety score based on proximity to unsafe zones
        var safetyScore = CalculateSafetyScore(coordinates, unsafeZones);

        return await Task.FromResult((
            Coordinates: coordinates,
            DistanceMeters: distance,
            DurationSeconds: (int)(distance / 15), // Assume 15 m/s average speed
            SafetyScore: safetyScore,
            UnsafeZonesAvoided: 0,
            NearbyZones: unsafeZones.Take(5).ToList()
        ));
    }

    private double CalculateSafetyScore(List<RoutePointDto> route, List<UnsafeZone> unsafeZones)
    {
        if (!unsafeZones.Any()) return 100.0;

        double minDistance = double.MaxValue;
        foreach (var point in route)
        {
            foreach (var zone in unsafeZones)
            {
                var distance = CalculateDistance(point.Lat, point.Lng, zone.CenterLat, zone.CenterLng);
                if (distance < minDistance)
                    minDistance = distance;
            }
        }

        // Score decreases as route gets closer to unsafe zones
        // 100 = very safe (>5km from any zone), 0 = very unsafe (inside zone)
        if (minDistance > 5000) return 100.0;
        if (minDistance < 100) return 0.0;
        return (minDistance / 5000.0) * 100.0;
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000; // Earth's radius in meters
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

