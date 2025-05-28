using BloodDonationAPI.DTOs.DonationDto;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IDonationService
    {
        Task<DonationResponseDto> CreateDonationAsync(DonationCreateDto dto);
        Task<DonationResponseDto?> GetDonationByIdAsync(int id);
        Task<IEnumerable<DonationResponseDto>> GetAllDonationsAsync();
        Task<bool> UpdateDonationAsync(DonationUpdateDto dto,int id);
        Task<bool> DeleteDonationAsync(int id);
    }
}