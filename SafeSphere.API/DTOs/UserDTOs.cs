using System.ComponentModel.DataAnnotations;

namespace SafeSphere.API.DTOs;

public class RegisterUserDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public string? EmergencyContacts { get; set; }
}

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? EmergencyContacts { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateUserDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? EmergencyContacts { get; set; }
}

