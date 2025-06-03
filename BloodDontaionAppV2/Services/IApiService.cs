using System.Collections.Generic;
using System.Threading.Tasks;
using BloodDonationAppV2.Models;

namespace BloodDonationAppV2.Services
{
    public interface IApiService
    {
        Task<LoginResponse> UserLoginAsync(string tcKimlikNo, string sifre);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<List<DonationRequest>> GetDonationRequestsAsync();
        Task<DonationRequest> GetDonationRequestByIdAsync(int id);
        Task<bool> CreateDonationRequestAsync(CreateDonationRequest request);
        Task<List<DonationRequest>> GetUserDonationRequestsAsync(string tcKimlikNo);
    }
}