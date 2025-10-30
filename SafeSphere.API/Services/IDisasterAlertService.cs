using SafeSphere.API.DTOs;

namespace SafeSphere.API.Services;

/// <summary>
/// Service interface for disaster alert operations
/// </summary>
public interface IDisasterAlertService
{
    Task<DisasterAlertResponseDto?> CreateDisasterAlertAsync(CreateDisasterAlertDto dto);
    Task<DisasterAlertResponseDto?> GetDisasterAlertByIdAsync(int id);
    Task<IEnumerable<DisasterAlertResponseDto>> GetDisasterAlertsAsync(GetDisasterAlertsRequestDto request);
    Task<IEnumerable<DisasterAlertResponseDto>> GetAllActiveDisasterAlertsAsync();
    Task<IEnumerable<DisasterAlertResponseDto>> GetDisasterAlertsByTypeAsync(string disasterType);
    Task<DisasterAlertResponseDto?> UpdateDisasterAlertAsync(int id, UpdateDisasterAlertDto dto);
    Task<bool> DeleteDisasterAlertAsync(int id);
    Task<bool> ConfirmDisasterAlertAsync(int alertId, int userId);
    Task<DisasterStatisticsDto> GetDisasterStatisticsAsync();
}

