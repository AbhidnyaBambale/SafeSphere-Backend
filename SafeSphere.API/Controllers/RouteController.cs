using Microsoft.AspNetCore.Mvc;
using SafeSphere.API.DTOs;
using SafeSphere.API.Services;

namespace SafeSphere.API.Controllers;

/// <summary>
/// Controller for safe route and unsafe zone management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RouteController : ControllerBase
{
    private readonly ISafeRouteService _routeService;
    private readonly ILogger<RouteController> _logger;

    public RouteController(ISafeRouteService routeService, ILogger<RouteController> logger)
    {
        _routeService = routeService;
        _logger = logger;
    }

    #region Safe Routes

    /// <summary>
    /// Get a safe route between two locations
    /// </summary>
    [HttpPost("safe")]
    [ProducesResponseType(typeof(SafeRouteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSafeRoute([FromQuery] int userId, [FromBody] GetSafeRouteRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _routeService.GetSafeRouteAsync(userId, request);
        
        if (result == null)
            return BadRequest(new { message = "Unable to calculate safe route or user not found" });

        return Ok(result);
    }

    /// <summary>
    /// Get route by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SafeRouteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRouteById(int id)
    {
        var result = await _routeService.GetRouteByIdAsync(id);
        
        if (result == null)
            return NotFound(new { message = "Route not found" });

        return Ok(result);
    }

    /// <summary>
    /// Get all routes for a user
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<SafeRouteResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRoutes(int userId)
    {
        var result = await _routeService.GetUserRoutesAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Get active routes for a user
    /// </summary>
    [HttpGet("user/{userId}/active")]
    [ProducesResponseType(typeof(IEnumerable<SafeRouteResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveUserRoutes(int userId)
    {
        var result = await _routeService.GetActiveUserRoutesAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Mark a route as completed
    /// </summary>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteRoute(int id)
    {
        var result = await _routeService.CompleteRouteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Route not found" });

        return NoContent();
    }

    /// <summary>
    /// Delete a route
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoute(int id)
    {
        var result = await _routeService.DeleteRouteAsync(id);
        
        if (!result)
            return NotFound(new { message = "Route not found" });

        return NoContent();
    }

    #endregion

    #region Unsafe Zones

    /// <summary>
    /// Create a new unsafe zone report
    /// </summary>
    [HttpPost("zones/unsafe")]
    [ProducesResponseType(typeof(UnsafeZoneResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnsafeZone([FromQuery] int userId, [FromBody] CreateUnsafeZoneDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _routeService.CreateUnsafeZoneAsync(userId, dto);
        
        if (result == null)
            return BadRequest(new { message = "Failed to create unsafe zone" });

        return CreatedAtAction(nameof(GetUnsafeZoneById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get unsafe zone by ID
    /// </summary>
    [HttpGet("zones/unsafe/{id}")]
    [ProducesResponseType(typeof(UnsafeZoneResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnsafeZoneById(int id)
    {
        var result = await _routeService.GetUnsafeZoneByIdAsync(id);
        
        if (result == null)
            return NotFound(new { message = "Unsafe zone not found" });

        return Ok(result);
    }

    /// <summary>
    /// Get all unsafe zones
    /// </summary>
    [HttpGet("zones/unsafe")]
    [ProducesResponseType(typeof(IEnumerable<UnsafeZoneResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUnsafeZones()
    {
        var result = await _routeService.GetAllUnsafeZonesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get active unsafe zones
    /// </summary>
    [HttpGet("zones/unsafe/active")]
    [ProducesResponseType(typeof(IEnumerable<UnsafeZoneResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveUnsafeZones()
    {
        var result = await _routeService.GetActiveUnsafeZonesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get unsafe zones near a location
    /// </summary>
    [HttpGet("zones/unsafe/nearby")]
    [ProducesResponseType(typeof(IEnumerable<UnsafeZoneResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNearbyUnsafeZones(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 10)
    {
        var result = await _routeService.GetUnsafeZonesByLocationAsync(latitude, longitude, radiusKm);
        return Ok(result);
    }

    /// <summary>
    /// Update an unsafe zone
    /// </summary>
    [HttpPatch("zones/unsafe/{id}")]
    [ProducesResponseType(typeof(UnsafeZoneResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnsafeZone(int id, [FromBody] UpdateUnsafeZoneDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _routeService.UpdateUnsafeZoneAsync(id, dto);
        
        if (result == null)
            return NotFound(new { message = "Unsafe zone not found" });

        return Ok(result);
    }

    /// <summary>
    /// Confirm an unsafe zone (user verification)
    /// </summary>
    [HttpPost("zones/unsafe/{id}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmUnsafeZone(int id, [FromQuery] int userId)
    {
        var result = await _routeService.ConfirmUnsafeZoneAsync(id, userId);
        
        if (!result)
            return NotFound(new { message = "Unsafe zone not found" });

        return NoContent();
    }

    /// <summary>
    /// Delete an unsafe zone
    /// </summary>
    [HttpDelete("zones/unsafe/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnsafeZone(int id)
    {
        var result = await _routeService.DeleteUnsafeZoneAsync(id);
        
        if (!result)
            return NotFound(new { message = "Unsafe zone not found" });

        return NoContent();
    }

    #endregion
}

