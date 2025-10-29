using SafeSphere.API.Models;

namespace SafeSphere.API.Repositories;

public interface IAlertRepository
{
    // Panic Alerts
    Task<PanicAlert?> GetPanicAlertByIdAsync(int id);
    Task<IEnumerable<PanicAlert>> GetAllPanicAlertsAsync();
    Task<IEnumerable<PanicAlert>> GetPanicAlertsByUserIdAsync(int userId);
    Task<IEnumerable<PanicAlert>> GetActivePanicAlertsAsync();
    Task<PanicAlert> CreatePanicAlertAsync(PanicAlert alert);
    Task<PanicAlert> UpdatePanicAlertAsync(PanicAlert alert);
    Task<bool> DeletePanicAlertAsync(int id);

    // SOS Alerts
    Task<SOSAlert?> GetSOSAlertByIdAsync(int id);
    Task<IEnumerable<SOSAlert>> GetAllSOSAlertsAsync();
    Task<IEnumerable<SOSAlert>> GetSOSAlertsByUserIdAsync(int userId);
    Task<IEnumerable<SOSAlert>> GetUnacknowledgedSOSAlertsAsync();
    Task<SOSAlert> CreateSOSAlertAsync(SOSAlert alert);
    Task<SOSAlert> UpdateSOSAlertAsync(SOSAlert alert);
    Task<bool> DeleteSOSAlertAsync(int id);
}

