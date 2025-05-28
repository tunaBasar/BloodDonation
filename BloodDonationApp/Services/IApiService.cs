using BloodDonationApp.Models;

namespace BloodDonationApp.Services
{
    public interface IApiService
    {
        Task<Response<UserResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
        Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(object registerRequest);
    }
}