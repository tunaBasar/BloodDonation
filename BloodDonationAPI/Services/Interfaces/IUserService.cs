using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.Entities;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateUserAsync(UserCreateDto dto);
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        Task<bool> UpdateUserAsync(int id, UserUpdateDto dto);

        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserLoginAsync(UserLoginDto dto);
        Task<bool> UserRegisterAsync(UserCreateDto dto);

        public User FindByTc(string tc);
    }
}
