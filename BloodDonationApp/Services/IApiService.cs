using BloodDonationApp.Models;

namespace BloodDonationApp.Services
{
    public interface IApiService
    {
        Task<Response<UserResponseDto>> LoginAsync(LoginRequestDto loginRequest);
        Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(object registerRequest);
        Task<(bool IsSuccess, string ErrorMessage)> CreateRequestAsync(object requestData);
        Task<Response<List<DonationRequest>>> GetDonationRequestsAsync(int Id);

        Task<Response<bool>> DoDonation(int Id,int UserId);
    }
}