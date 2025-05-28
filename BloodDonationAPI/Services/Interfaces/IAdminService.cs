using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.DTOs.AdminDto;
using BloodDonationAPI.DTOs.PlacesDto;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<UserResponseDto> CreateUserAsync(UserCreateDto dto);
        Task<bool> UpdateUserAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<DonationResponseDto> CreateDonationAsync(DonationCreateDto dto);
        Task<bool> DeleteDonationAsync(int id);
        Task<bool> UpdateDonationAsync(DonationUpdateDto dto, int id);
        Task<DonationResponseDto?> GetDonationByIdAsync(int id);
        Task<IEnumerable<DonationResponseDto>> GetAllDonationsAsync();

        Task<RequestResponseDto> CreateRequestAsync(RequestCreateDto requestCreateDto);
        Task<bool> DeleteRequestAsync(int id);
        Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync();
        Task<RequestResponseDto?> GetRequestByIdAsync(int id);
        Task<bool> UpdateRequestAsync(int id, RequestUpdateDto requestUpdateDto);

        Task<PlacesDto> CreatePlacesAsync(PlacesDto dto);
        Task<PlacesDto?> GetPlacesByIdAsync(int id);
        Task<IEnumerable<PlacesDto>> GetAllPlacesAsync();
        Task<bool> UpdatePlacesAsync(int id, PlacesDto dto);
        Task<bool> DeletePlacesAsync(int id);

        Task<bool> AdminLoginAsync(AdminLoginDto dto);
    }

}