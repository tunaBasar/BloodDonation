using BloodDonationAPI.DataAccess;
using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Extensions;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services
{


    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;
        private readonly IDonationService donationService;


        public RequestService(ApplicationDbContext context,
        IUserService userService, IDonationService donationService)
        {
            this.context = context;
            this.userService = userService;
            this.donationService = donationService;

        }

        public async Task<RequestResponseDto> CreateRequestAsync(RequestCreateDto requestCreateDto)
        {
            if (requestCreateDto == null)
            {
                throw new ArgumentNullException(nameof(requestCreateDto), " Cannot be null");
            }
            if (requestCreateDto.UserId <= 0)
            {
                throw new ArgumentNullException(nameof(requestCreateDto.UserId), " Cannot be null");
            }
            var request = requestCreateDto.ToEntity();
            context.Requests.Add(request);
            await context.SaveChangesAsync();
            return request.ToResponseDto();
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid request id ", nameof(id));
            }
            var request = FindRequestById(id);
            if (request == null)
            {
                return false;
            }
            context.Requests.Remove(request);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return new List<RequestResponseDto>();
            }
            var request = await context.Requests
            .Where(r => r.BloodType == user.BloodType && r.IsActive != false)
            .ToListAsync();
            return request.Select(r => r.ToResponseDto());
        }

        public async Task<RequestResponseDto?> GetRequestByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid request id ", nameof(id));
            }
            var request = await context.Requests.FindAsync(id);
            if (request == null)
                throw new ArgumentException(nameof(request), " Cannot find");
            return request.ToResponseDto();
        }

        public async Task<bool> UpdateRequestAsync(int id, RequestUpdateDto requestUpdateDto)
        {
            if (requestUpdateDto == null)
            {
                return false;
            }
            var oldRequest = FindRequestById(id);
            context.Entry(oldRequest).CurrentValues.SetValues(requestUpdateDto);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactiveRequest(int id, DeactiveRequestDto dto)
        {
            var request = FindRequestById(id);
            request.IsActive = dto.IsActive;
            await context.SaveChangesAsync();
            await donationService.CreateDonationAsync(dto.ToDonationCreateDto());
            return true;
        }
        public Request FindRequestById(int id)
        {
            var request = context.Requests.Find(id);
            if (request == null)
                throw new ArgumentNullException(nameof(id), "cannot find");
            return request;
        }
    }
}