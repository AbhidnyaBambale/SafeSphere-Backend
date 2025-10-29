using AutoMapper;
using SafeSphere.API.DTOs;
using SafeSphere.API.Models;
using SafeSphere.API.Repositories;

namespace SafeSphere.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserResponseDto?> RegisterAsync(RegisterUserDto dto)
    {
        try
        {
            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists", dto.Email);
                return null;
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = passwordHash,
                EmergencyContacts = dto.EmergencyContacts,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);
            _logger.LogInformation("User registered successfully: {Email}", dto.Email);

            return _mapper.Map<UserResponseDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", dto.Email);
            throw;
        }
    }

    public async Task<UserResponseDto?> LoginAsync(LoginUserDto dto)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for email {Email}", dto.Email);
                return null;
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: Invalid password for email {Email}", dto.Email);
                return null;
            }

            _logger.LogInformation("User logged in successfully: {Email}", dto.Email);
            return _mapper.Map<UserResponseDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user: {Email}", dto.Email);
            throw;
        }
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID: {Id}", id);
            throw;
        }
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, UpdateUserDto dto)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Update failed: User not found with ID {Id}", id);
                return null;
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                user.Phone = dto.Phone;

            if (dto.EmergencyContacts != null)
                user.EmergencyContacts = dto.EmergencyContacts;

            var updatedUser = await _userRepository.UpdateAsync(user);
            _logger.LogInformation("User updated successfully: {Id}", id);

            return _mapper.Map<UserResponseDto>(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _userRepository.DeleteAsync(id);
            if (result)
                _logger.LogInformation("User deleted successfully: {Id}", id);
            else
                _logger.LogWarning("Delete failed: User not found with ID {Id}", id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Id}", id);
            throw;
        }
    }
}

