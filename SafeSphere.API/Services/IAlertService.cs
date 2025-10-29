using SafeSphere.API.DTOs;

namespace SafeSphere.API.Services;

public interface IAlertService
{
    // Panic Alerts
    Task<PanicAlertResponseDto?> CreatePanicAlertAsync(int userId, CreatePanicAlertDto dto);
    Task<PanicAlertResponseDto?> GetPanicAlertByIdAsync(int id);
    Task<IEnumerable<PanicAlertResponseDto>> GetAllPanicAlertsAsync();
    Task<IEnumerable<PanicAlertResponseDto>> GetPanicAlertsByUserIdAsync(int userId);
    Task<IEnumerable<PanicAlertResponseDto>> GetActivePanicAlertsAsync();
    Task<PanicAlertResponseDto?> UpdatePanicAlertStatusAsync(int id, UpdatePanicAlertStatusDto dto);
    Task<bool> DeletePanicAlertAsync(int id);

    // SOS Alerts
    Task<SOSAlertResponseDto?> CreateSOSAlertAsync(int userId, CreateSOSAlertDto dto);
    Task<SOSAlertResponseDto?> GetSOSAlertByIdAsync(int id);
    Task<IEnumerable<SOSAlertResponseDto>> GetAllSOSAlertsAsync();
    Task<IEnumerable<SOSAlertResponseDto>> GetSOSAlertsByUserIdAsync(int userId);
    Task<IEnumerable<SOSAlertResponseDto>> GetUnacknowledgedSOSAlertsAsync();
    Task<SOSAlertResponseDto?> AcknowledgeSOSAlertAsync(int id, AcknowledgeSOSAlertDto dto);
    Task<bool> DeleteSOSAlertAsync(int id);
}

