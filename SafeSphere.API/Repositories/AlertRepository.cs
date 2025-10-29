using Microsoft.EntityFrameworkCore;
using SafeSphere.API.Data;
using SafeSphere.API.Models;

namespace SafeSphere.API.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly SafeSphereDbContext _context;

    public AlertRepository(SafeSphereDbContext context)
    {
        _context = context;
    }

    // Panic Alerts
    public async Task<PanicAlert?> GetPanicAlertByIdAsync(int id)
    {
        return await _context.PanicAlerts
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PanicAlert>> GetAllPanicAlertsAsync()
    {
        return await _context.PanicAlerts
            .Include(p => p.User)
            .OrderByDescending(p => p.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<PanicAlert>> GetPanicAlertsByUserIdAsync(int userId)
    {
        return await _context.PanicAlerts
            .Include(p => p.User)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<PanicAlert>> GetActivePanicAlertsAsync()
    {
        return await _context.PanicAlerts
            .Include(p => p.User)
            .Where(p => p.Status == "Active")
            .OrderByDescending(p => p.Timestamp)
            .ToListAsync();
    }

    public async Task<PanicAlert> CreatePanicAlertAsync(PanicAlert alert)
    {
        _context.PanicAlerts.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<PanicAlert> UpdatePanicAlertAsync(PanicAlert alert)
    {
        _context.PanicAlerts.Update(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<bool> DeletePanicAlertAsync(int id)
    {
        var alert = await _context.PanicAlerts.FindAsync(id);
        if (alert == null) return false;

        _context.PanicAlerts.Remove(alert);
        await _context.SaveChangesAsync();
        return true;
    }

    // SOS Alerts
    public async Task<SOSAlert?> GetSOSAlertByIdAsync(int id)
    {
        return await _context.SOSAlerts
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<SOSAlert>> GetAllSOSAlertsAsync()
    {
        return await _context.SOSAlerts
            .Include(s => s.User)
            .OrderByDescending(s => s.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<SOSAlert>> GetSOSAlertsByUserIdAsync(int userId)
    {
        return await _context.SOSAlerts
            .Include(s => s.User)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<SOSAlert>> GetUnacknowledgedSOSAlertsAsync()
    {
        return await _context.SOSAlerts
            .Include(s => s.User)
            .Where(s => !s.Acknowledged)
            .OrderByDescending(s => s.Timestamp)
            .ToListAsync();
    }

    public async Task<SOSAlert> CreateSOSAlertAsync(SOSAlert alert)
    {
        _context.SOSAlerts.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<SOSAlert> UpdateSOSAlertAsync(SOSAlert alert)
    {
        _context.SOSAlerts.Update(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<bool> DeleteSOSAlertAsync(int id)
    {
        var alert = await _context.SOSAlerts.FindAsync(id);
        if (alert == null) return false;

        _context.SOSAlerts.Remove(alert);
        await _context.SaveChangesAsync();
        return true;
    }
}

