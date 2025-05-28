using BloodDonationAPI.DTOs.RequestDto;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IRequestService
    {
        Task<RequestResponseDto> CreateRequestAsync(RequestCreateDto requestCreateDto);
        Task<RequestResponseDto?> GetRequestByIdAsync(int id);
        Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync();
        Task<bool> UpdateRequestAsync(int id,RequestUpdateDto requestUpdateDto);
        Task<bool> DeleteRequestAsync(int id);
    }
}