using Microsoft.AspNetCore.Mvc;
using SafeSphere.API.DTOs;
using SafeSphere.API.Services;

namespace SafeSphere.API.Controllers;

/// <summary>
/// Controller for disaster alerts and emergency notifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DisasterController : ControllerBase
{
    private readonly IDisasterAlertService _disasterService;
    private readonly ILogger<DisasterController> _logger;

    public DisasterController(IDisasterAlertService disasterService, ILogger<DisasterController> logger)
    {
        _disasterService = disasterService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new disaster alert
    /// </summary>
    [HttpPost("alerts")]
    [ProducesResponseType(typeof(DisasterAlertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDisasterAlert([FromBody] CreateDisasterAlertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _disasterService.CreateDisasterAlertAsync(dto);
        
        if (result == null)
            return BadRequest(new { message = "Failed to create disaster alert" });

        return CreatedAtAction(nameof(GetDisasterAlertById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get disaster alert by ID
    /// </summary>
    [HttpGet("alerts/{id}")]
    [ProducesResponseType(typeof(DisasterAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDisasterAlertById(int id)
    {
        var result = await _disasterService.GetDisasterAlertByIdAsync(id);
        
        if (result == null)
            return NotFound(new { message = "Disaster alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Get disaster alerts for a location
    /// </summary>
    [HttpPost("alerts/search")]
    [ProducesResponseType(typeof(IEnumerable<DisasterAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDisasterAlerts([FromBody] GetDisasterAlertsRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _disasterService.GetDisasterAlertsAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get all active disaster alerts
    /// </summary>
    [HttpGet("alerts/active")]
    [ProducesResponseType(typeof(IEnumerable<DisasterAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveDisasterAlerts()
    {
        var result = await _disasterService.GetAllActiveDisasterAlertsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get disaster alerts by type
    /// </summary>
    [HttpGet("alerts/type/{disasterType}")]
    [ProducesResponseType(typeof(IEnumerable<DisasterAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDisasterAlertsByType(string disasterType)
    {
        var result = await _disasterService.GetDisasterAlertsByTypeAsync(disasterType);
        return Ok(result);
    }

    /// <summary>
    /// Update a disaster alert
    /// </summary>
    [HttpPatch("alerts/{id}")]
    [ProducesResponseType(typeof(DisasterAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDisasterAlert(int id, [FromBody] UpdateDisasterAlertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _disasterService.UpdateDisasterAlertAsync(id, dto);
        
        if (result == null)
            return NotFound(new { message = "Disaster alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Confirm a disaster alert (user verification)
    /// </summary>
    [HttpPost("alerts/{id}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmDisasterAlert(int id, [FromQuery] int userId)
    {
        var result = await _disasterService.ConfirmDisasterAlertAsync(id, userId);
        
        if (!result)
            return NotFound(new { message = "Disaster alert not found" });

        return NoContent();
    }

    /// <summary>
    /// Delete a disaster alert
    /// </summary>
    [HttpDelete("alerts/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDisasterAlert(int id)
    {
        var result = await _disasterService.DeleteDisasterAlertAsync(id);
        
        if (!result)
            return NotFound(new { message = "Disaster alert not found" });

        return NoContent();
    }

    /// <summary>
    /// Get disaster statistics
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(DisasterStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDisasterStatistics()
    {
        var result = await _disasterService.GetDisasterStatisticsAsync();
        return Ok(result);
    }
}

