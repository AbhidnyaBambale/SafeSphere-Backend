using AutoMapper;
using SafeSphere.API.DTOs;
using SafeSphere.API.Models;
using SafeSphere.API.Repositories;

namespace SafeSphere.API.Services;

/// <summary>
/// Service implementation for disaster alert operations
/// </summary>
public class DisasterAlertService : IDisasterAlertService
{
    private readonly IWeatherAlertRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DisasterAlertService> _logger;

    public DisasterAlertService(
        IWeatherAlertRepository repository,
        IMapper mapper,
        ILogger<DisasterAlertService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DisasterAlertResponseDto?> CreateDisasterAlertAsync(CreateDisasterAlertDto dto)
    {
        try
        {
            var alert = new DisasterAlert
            {
                Title = dto.Title,
                Description = dto.Description,
                DisasterType = dto.DisasterType,
                AffectedArea = dto.AffectedArea,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                AffectedRadiusKm = dto.AffectedRadiusKm,
                Severity = dto.Severity,
                Status = "Active",
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = dto.ExpiresAt,
                ExternalAlertId = dto.ExternalAlertId,
                Source = dto.Source,
                SafetyInstructions = dto.SafetyInstructions,
                EmergencyContactInfo = dto.EmergencyContactInfo,
                ConfirmationCount = 0
            };

            var created = await _repository.CreateDisasterAlertAsync(alert);
            return created != null ? _mapper.Map<DisasterAlertResponseDto>(created) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating disaster alert");
            return null;
        }
    }

    public async Task<DisasterAlertResponseDto?> GetDisasterAlertByIdAsync(int id)
    {
        try
        {
            var alert = await _repository.GetDisasterAlertByIdAsync(id);
            return alert != null ? _mapper.Map<DisasterAlertResponseDto>(alert) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster alert by ID: {AlertId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<DisasterAlertResponseDto>> GetDisasterAlertsAsync(GetDisasterAlertsRequestDto request)
    {
        try
        {
            var alerts = await _repository.GetDisasterAlertsByLocationAsync(
                request.Latitude, request.Longitude, request.RadiusKm);

            var response = _mapper.Map<List<DisasterAlertResponseDto>>(alerts);

            // Calculate distance and check if user is in affected area
            foreach (var alert in response)
            {
                var distance = CalculateDistance(request.Latitude, request.Longitude, 
                                                alert.Latitude, alert.Longitude) / 1000; // Convert to km
                alert.DistanceKm = distance;
                
                if (alert.AffectedRadiusKm.HasValue)
                {
                    alert.IsUserInAffectedArea = distance <= alert.AffectedRadiusKm.Value;
                }
            }

            // Filter by type if specified
            if (!string.IsNullOrEmpty(request.DisasterType))
            {
                response = response.Where(a => a.DisasterType.Equals(request.DisasterType, 
                    StringComparison.OrdinalIgnoreCase)).ToList();
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
            _logger.LogError(ex, "Error getting disaster alerts");
            return Enumerable.Empty<DisasterAlertResponseDto>();
        }
    }

    public async Task<IEnumerable<DisasterAlertResponseDto>> GetAllActiveDisasterAlertsAsync()
    {
        try
        {
            var alerts = await _repository.GetActiveDisasterAlertsAsync();
            return _mapper.Map<IEnumerable<DisasterAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all active disaster alerts");
            return Enumerable.Empty<DisasterAlertResponseDto>();
        }
    }

    public async Task<IEnumerable<DisasterAlertResponseDto>> GetDisasterAlertsByTypeAsync(string disasterType)
    {
        try
        {
            var alerts = await _repository.GetDisasterAlertsByTypeAsync(disasterType);
            return _mapper.Map<IEnumerable<DisasterAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster alerts by type: {Type}", disasterType);
            return Enumerable.Empty<DisasterAlertResponseDto>();
        }
    }

    public async Task<DisasterAlertResponseDto?> UpdateDisasterAlertAsync(int id, UpdateDisasterAlertDto dto)
    {
        try
        {
            var alert = await _repository.GetDisasterAlertByIdAsync(id);
            if (alert == null) return null;

            if (!string.IsNullOrEmpty(dto.Description))
                alert.Description = dto.Description;

            if (!string.IsNullOrEmpty(dto.Status))
                alert.Status = dto.Status;

            if (!string.IsNullOrEmpty(dto.Severity))
                alert.Severity = dto.Severity;

            if (dto.ExpiresAt.HasValue)
                alert.ExpiresAt = dto.ExpiresAt;

            if (!string.IsNullOrEmpty(dto.SafetyInstructions))
                alert.SafetyInstructions = dto.SafetyInstructions;

            alert.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateDisasterAlertAsync(alert);
            return updated != null ? _mapper.Map<DisasterAlertResponseDto>(updated) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating disaster alert: {AlertId}", id);
            return null;
        }
    }

    public async Task<bool> DeleteDisasterAlertAsync(int id)
    {
        try
        {
            return await _repository.DeleteDisasterAlertAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting disaster alert: {AlertId}", id);
            return false;
        }
    }

    public async Task<bool> ConfirmDisasterAlertAsync(int alertId, int userId)
    {
        try
        {
            return await _repository.IncrementDisasterConfirmationAsync(alertId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming disaster alert: {AlertId}", alertId);
            return false;
        }
    }

    public async Task<DisasterStatisticsDto> GetDisasterStatisticsAsync()
    {
        try
        {
            var allAlerts = await _repository.GetActiveDisasterAlertsAsync();
            var alertsList = allAlerts.ToList();

            var stats = new DisasterStatisticsDto
            {
                TotalActiveAlerts = alertsList.Count,
                CriticalAlerts = alertsList.Count(a => a.Severity == "Extreme" || a.Severity == "Severe"),
                AlertsByType = alertsList.GroupBy(a => a.DisasterType)
                                         .ToDictionary(g => g.Key, g => g.Count()),
                AlertsBySeverity = alertsList.GroupBy(a => a.Severity)
                                             .ToDictionary(g => g.Key, g => g.Count()),
                RecentAlerts = _mapper.Map<List<DisasterAlertResponseDto>>(alertsList.Take(10))
            };

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting disaster statistics");
            return new DisasterStatisticsDto();
        }
    }

    #region Helper Methods

    private List<DisasterAlertResponseDto> FilterBySeverity(List<DisasterAlertResponseDto> alerts, string minimumSeverity)
    {
        var severityOrder = new Dictionary<string, int>
        {
            { "Low", 1 },
            { "Moderate", 2 },
            { "High", 3 },
            { "Severe", 4 },
            { "Extreme", 5 }
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

