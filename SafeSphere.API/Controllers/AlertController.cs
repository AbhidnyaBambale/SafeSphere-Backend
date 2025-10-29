using Microsoft.AspNetCore.Mvc;
using SafeSphere.API.DTOs;
using SafeSphere.API.Services;

namespace SafeSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertController : ControllerBase
{
    private readonly IAlertService _alertService;
    private readonly ILogger<AlertController> _logger;

    public AlertController(IAlertService alertService, ILogger<AlertController> logger)
    {
        _alertService = alertService;
        _logger = logger;
    }

    #region Panic Alerts

    /// <summary>
    /// Create a new panic alert
    /// </summary>
    [HttpPost("panic")]
    [ProducesResponseType(typeof(PanicAlertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePanicAlert([FromQuery] int userId, [FromBody] CreatePanicAlertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _alertService.CreatePanicAlertAsync(userId, dto);
        
        if (result == null)
            return BadRequest(new { message = "User not found or failed to create alert" });

        return CreatedAtAction(nameof(GetPanicAlertById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get panic alert by ID
    /// </summary>
    [HttpGet("panic/{id}")]
    [ProducesResponseType(typeof(PanicAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPanicAlertById(int id)
    {
        var result = await _alertService.GetPanicAlertByIdAsync(id);
        
        if (result == null)
            return NotFound(new { message = "Panic alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Get all panic alerts
    /// </summary>
    [HttpGet("panic")]
    [ProducesResponseType(typeof(IEnumerable<PanicAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPanicAlerts()
    {
        var result = await _alertService.GetAllPanicAlertsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get panic alerts by user ID
    /// </summary>
    [HttpGet("panic/user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<PanicAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPanicAlertsByUserId(int userId)
    {
        var result = await _alertService.GetPanicAlertsByUserIdAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Get all active panic alerts
    /// </summary>
    [HttpGet("panic/active")]
    [ProducesResponseType(typeof(IEnumerable<PanicAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivePanicAlerts()
    {
        var result = await _alertService.GetActivePanicAlertsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Update panic alert status
    /// </summary>
    [HttpPatch("panic/{id}/status")]
    [ProducesResponseType(typeof(PanicAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePanicAlertStatus(int id, [FromBody] UpdatePanicAlertStatusDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _alertService.UpdatePanicAlertStatusAsync(id, dto);
        
        if (result == null)
            return NotFound(new { message = "Panic alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Delete a panic alert
    /// </summary>
    [HttpDelete("panic/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePanicAlert(int id)
    {
        var result = await _alertService.DeletePanicAlertAsync(id);
        
        if (!result)
            return NotFound(new { message = "Panic alert not found" });

        return NoContent();
    }

    #endregion

    #region SOS Alerts

    /// <summary>
    /// Create a new SOS alert
    /// </summary>
    [HttpPost("sos")]
    [ProducesResponseType(typeof(SOSAlertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSOSAlert([FromQuery] int userId, [FromBody] CreateSOSAlertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _alertService.CreateSOSAlertAsync(userId, dto);
        
        if (result == null)
            return BadRequest(new { message = "User not found or failed to create alert" });

        return CreatedAtAction(nameof(GetSOSAlertById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get SOS alert by ID
    /// </summary>
    [HttpGet("sos/{id}")]
    [ProducesResponseType(typeof(SOSAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSOSAlertById(int id)
    {
        var result = await _alertService.GetSOSAlertByIdAsync(id);
        
        if (result == null)
            return NotFound(new { message = "SOS alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Get all SOS alerts
    /// </summary>
    [HttpGet("sos")]
    [ProducesResponseType(typeof(IEnumerable<SOSAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSOSAlerts()
    {
        var result = await _alertService.GetAllSOSAlertsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get SOS alerts by user ID
    /// </summary>
    [HttpGet("sos/user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<SOSAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSOSAlertsByUserId(int userId)
    {
        var result = await _alertService.GetSOSAlertsByUserIdAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Get all unacknowledged SOS alerts
    /// </summary>
    [HttpGet("sos/unacknowledged")]
    [ProducesResponseType(typeof(IEnumerable<SOSAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnacknowledgedSOSAlerts()
    {
        var result = await _alertService.GetUnacknowledgedSOSAlertsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Acknowledge an SOS alert
    /// </summary>
    [HttpPatch("sos/{id}/acknowledge")]
    [ProducesResponseType(typeof(SOSAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcknowledgeSOSAlert(int id, [FromBody] AcknowledgeSOSAlertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _alertService.AcknowledgeSOSAlertAsync(id, dto);
        
        if (result == null)
            return NotFound(new { message = "SOS alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Delete an SOS alert
    /// </summary>
    [HttpDelete("sos/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSOSAlert(int id)
    {
        var result = await _alertService.DeleteSOSAlertAsync(id);
        
        if (!result)
            return NotFound(new { message = "SOS alert not found" });

        return NoContent();
    }

    #endregion
}

