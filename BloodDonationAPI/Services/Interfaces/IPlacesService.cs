using BloodDonationAPI.DTOs.PlacesDto;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IPlacesService
    {
        Task<PlacesDto> CreatePlacesAsync(PlacesDto dto);
        Task<PlacesDto?> GetPlacesByIdAsync(int id);
        Task<IEnumerable<PlacesDto>> GetAllPlacesAsync();
        Task<bool> UpdatePlacesAsync(int id,PlacesDto dto);
        Task<bool> DeletePlacesAsync(int id);    
    }
}