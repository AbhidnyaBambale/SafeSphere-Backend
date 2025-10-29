using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.DTOs;

// Panic Alert DTOs
public class CreatePanicAlertDto
{
    [Required]
    public double LocationLat { get; set; }

    [Required]
    public double LocationLng { get; set; }

    [MaxLength(500)]
    public string? AdditionalInfo { get; set; }
}

public class PanicAlertResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public double LocationLat { get; set; }
    public double LocationLng { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AdditionalInfo { get; set; }
}

public class UpdatePanicAlertStatusDto
{
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty; // Active, Resolved, Cancelled
}

// SOS Alert DTOs
public class CreateSOSAlertDto
{
    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Location { get; set; } = string.Empty;

    public double? LocationLat { get; set; }

    public double? LocationLng { get; set; }
}

public class SOSAlertResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double? LocationLat { get; set; }
    public double? LocationLng { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Acknowledged { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
}

public class AcknowledgeSOSAlertDto
{
    [Required]
    public bool Acknowledged { get; set; }
}

