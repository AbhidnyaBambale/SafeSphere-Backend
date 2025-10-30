using Microsoft.AspNetCore.Mvc;
using SafeSphere.API.DTOs;
using SafeSphere.API.Services;

namespace SafeSphere.API.Controllers;

/// <summary>
/// Controller for weather alerts and current weather information
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherAlertService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherAlertService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    /// <summary>
    /// Get current weather for a location
    /// </summary>
    [HttpGet("current")]
    [ProducesResponseType(typeof(CurrentWeatherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCurrentWeather(
        [FromQuery] double latitude,
        [FromQuery] double longitude)
    {
        var result = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
        
        if (result == null)
            return BadRequest(new { message = "Unable to fetch weather data" });

        return Ok(result);
    }

    /// <summary>
    /// Get weather alerts for a location
    /// </summary>
    [HttpPost("alerts")]
    [ProducesResponseType(typeof(IEnumerable<WeatherAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeatherAlerts([FromBody] GetWeatherAlertsRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _weatherService.GetWeatherAlertsAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get all active weather alerts
    /// </summary>
    [HttpGet("alerts/active")]
    [ProducesResponseType(typeof(IEnumerable<WeatherAlertResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveWeatherAlerts()
    {
        var result = await _weatherService.GetAllActiveWeatherAlertsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get weather alert by ID
    /// </summary>
    [HttpGet("alerts/{id}")]
    [ProducesResponseType(typeof(WeatherAlertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWeatherAlertById(int id)
    {
        var result = await _weatherService.GetWeatherAlertByIdAsync(id);
        
        if (result == null)
            return NotFound(new { message = "Weather alert not found" });

        return Ok(result);
    }

    /// <summary>
    /// Create a manual weather alert (admin/system use)
    /// </summary>
    [HttpPost("alerts/create")]
    [ProducesResponseType(typeof(WeatherAlertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWeatherAlert([FromBody] CreateWeatherAlertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _weatherService.CreateWeatherAlertAsync(dto);
        
        if (result == null)
            return BadRequest(new { message = "Failed to create weather alert" });

        return CreatedAtAction(nameof(GetWeatherAlertById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Delete a weather alert
    /// </summary>
    [HttpDelete("alerts/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWeatherAlert(int id)
    {
        var result = await _weatherService.DeleteWeatherAlertAsync(id);
        
        if (!result)
            return NotFound(new { message = "Weather alert not found" });

        return NoContent();
    }
}

