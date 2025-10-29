using SafeSphere.API.DTOs;

namespace SafeSphere.API.Services;

public interface IUserService
{
    Task<UserResponseDto?> RegisterAsync(RegisterUserDto dto);
    Task<UserResponseDto?> LoginAsync(LoginUserDto dto);
    Task<UserResponseDto?> GetByIdAsync(int id);
    Task<UserResponseDto?> UpdateAsync(int id, UpdateUserDto dto);
    Task<bool> DeleteAsync(int id);
}

