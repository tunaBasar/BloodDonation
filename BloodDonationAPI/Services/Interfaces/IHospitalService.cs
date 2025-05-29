using BloodDonationAPI.DTOs.PlacesDto;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IHospitalService
    {
        Task<HospitalsDto> CreatePlacesAsync(HospitalsDto dto);
        Task<HospitalsDto?> GetPlacesByIdAsync(int id);
        Task<IEnumerable<HospitalsDto>> GetAllPlacesAsync();
        Task<bool> UpdatePlacesAsync(int id,HospitalsDto dto);
        Task<bool> DeletePlacesAsync(int id);    
    }
}