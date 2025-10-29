using AutoMapper;
using SafeSphere.API.DTOs;
using SafeSphere.API.Models;
using SafeSphere.API.Repositories;

namespace SafeSphere.API.Services;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AlertService> _logger;

    public AlertService(IAlertRepository alertRepository, IUserRepository userRepository, IMapper mapper, ILogger<AlertService> logger)
    {
        _alertRepository = alertRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // Panic Alerts
    public async Task<PanicAlertResponseDto?> CreatePanicAlertAsync(int userId, CreatePanicAlertDto dto)
    {
        try
        {
            // Verify user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Create panic alert failed: User not found with ID {UserId}", userId);
                return null;
            }

            var alert = new PanicAlert
            {
                UserId = userId,
                LocationLat = dto.LocationLat,
                LocationLng = dto.LocationLng,
                AdditionalInfo = dto.AdditionalInfo,
                Timestamp = DateTime.UtcNow,
                Status = "Active"
            };

            var createdAlert = await _alertRepository.CreatePanicAlertAsync(alert);
            
            // Reload to get user data
            createdAlert = await _alertRepository.GetPanicAlertByIdAsync(createdAlert.Id);
            
            _logger.LogInformation("Panic alert created: ID {Id} for User {UserId}", createdAlert!.Id, userId);
            return _mapper.Map<PanicAlertResponseDto>(createdAlert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating panic alert for user {UserId}", userId);
            throw;
        }
    }

    public async Task<PanicAlertResponseDto?> GetPanicAlertByIdAsync(int id)
    {
        try
        {
            var alert = await _alertRepository.GetPanicAlertByIdAsync(id);
            return alert == null ? null : _mapper.Map<PanicAlertResponseDto>(alert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting panic alert by ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PanicAlertResponseDto>> GetAllPanicAlertsAsync()
    {
        try
        {
            var alerts = await _alertRepository.GetAllPanicAlertsAsync();
            return _mapper.Map<IEnumerable<PanicAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all panic alerts");
            throw;
        }
    }

    public async Task<IEnumerable<PanicAlertResponseDto>> GetPanicAlertsByUserIdAsync(int userId)
    {
        try
        {
            var alerts = await _alertRepository.GetPanicAlertsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PanicAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting panic alerts for user {UserId}", userId);
            throw;
        }
    }

    public async Task<IEnumerable<PanicAlertResponseDto>> GetActivePanicAlertsAsync()
    {
        try
        {
            var alerts = await _alertRepository.GetActivePanicAlertsAsync();
            return _mapper.Map<IEnumerable<PanicAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active panic alerts");
            throw;
        }
    }

    public async Task<PanicAlertResponseDto?> UpdatePanicAlertStatusAsync(int id, UpdatePanicAlertStatusDto dto)
    {
        try
        {
            var alert = await _alertRepository.GetPanicAlertByIdAsync(id);
            if (alert == null)
            {
                _logger.LogWarning("Update panic alert failed: Alert not found with ID {Id}", id);
                return null;
            }

            alert.Status = dto.Status;
            var updatedAlert = await _alertRepository.UpdatePanicAlertAsync(alert);
            
            _logger.LogInformation("Panic alert status updated: ID {Id} to {Status}", id, dto.Status);
            return _mapper.Map<PanicAlertResponseDto>(updatedAlert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating panic alert status: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeletePanicAlertAsync(int id)
    {
        try
        {
            var result = await _alertRepository.DeletePanicAlertAsync(id);
            if (result)
                _logger.LogInformation("Panic alert deleted: {Id}", id);
            else
                _logger.LogWarning("Delete panic alert failed: Alert not found with ID {Id}", id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting panic alert: {Id}", id);
            throw;
        }
    }

    // SOS Alerts
    public async Task<SOSAlertResponseDto?> CreateSOSAlertAsync(int userId, CreateSOSAlertDto dto)
    {
        try
        {
            // Verify user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Create SOS alert failed: User not found with ID {UserId}", userId);
                return null;
            }

            var alert = new SOSAlert
            {
                UserId = userId,
                Message = dto.Message,
                Location = dto.Location,
                LocationLat = dto.LocationLat,
                LocationLng = dto.LocationLng,
                Timestamp = DateTime.UtcNow,
                Acknowledged = false
            };

            var createdAlert = await _alertRepository.CreateSOSAlertAsync(alert);
            
            // Reload to get user data
            createdAlert = await _alertRepository.GetSOSAlertByIdAsync(createdAlert.Id);
            
            _logger.LogInformation("SOS alert created: ID {Id} for User {UserId}", createdAlert!.Id, userId);
            return _mapper.Map<SOSAlertResponseDto>(createdAlert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SOS alert for user {UserId}", userId);
            throw;
        }
    }

    public async Task<SOSAlertResponseDto?> GetSOSAlertByIdAsync(int id)
    {
        try
        {
            var alert = await _alertRepository.GetSOSAlertByIdAsync(id);
            return alert == null ? null : _mapper.Map<SOSAlertResponseDto>(alert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SOS alert by ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<SOSAlertResponseDto>> GetAllSOSAlertsAsync()
    {
        try
        {
            var alerts = await _alertRepository.GetAllSOSAlertsAsync();
            return _mapper.Map<IEnumerable<SOSAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all SOS alerts");
            throw;
        }
    }

    public async Task<IEnumerable<SOSAlertResponseDto>> GetSOSAlertsByUserIdAsync(int userId)
    {
        try
        {
            var alerts = await _alertRepository.GetSOSAlertsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<SOSAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SOS alerts for user {UserId}", userId);
            throw;
        }
    }

    public async Task<IEnumerable<SOSAlertResponseDto>> GetUnacknowledgedSOSAlertsAsync()
    {
        try
        {
            var alerts = await _alertRepository.GetUnacknowledgedSOSAlertsAsync();
            return _mapper.Map<IEnumerable<SOSAlertResponseDto>>(alerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unacknowledged SOS alerts");
            throw;
        }
    }

    public async Task<SOSAlertResponseDto?> AcknowledgeSOSAlertAsync(int id, AcknowledgeSOSAlertDto dto)
    {
        try
        {
            var alert = await _alertRepository.GetSOSAlertByIdAsync(id);
            if (alert == null)
            {
                _logger.LogWarning("Acknowledge SOS alert failed: Alert not found with ID {Id}", id);
                return null;
            }

            alert.Acknowledged = dto.Acknowledged;
            alert.AcknowledgedAt = dto.Acknowledged ? DateTime.UtcNow : null;
            
            var updatedAlert = await _alertRepository.UpdateSOSAlertAsync(alert);
            
            _logger.LogInformation("SOS alert acknowledged: ID {Id}", id);
            return _mapper.Map<SOSAlertResponseDto>(updatedAlert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error acknowledging SOS alert: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteSOSAlertAsync(int id)
    {
        try
        {
            var result = await _alertRepository.DeleteSOSAlertAsync(id);
            if (result)
                _logger.LogInformation("SOS alert deleted: {Id}", id);
            else
                _logger.LogWarning("Delete SOS alert failed: Alert not found with ID {Id}", id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SOS alert: {Id}", id);
            throw;
        }
    }
}

