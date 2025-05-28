using BloodDonationAPI.DataAccess;
using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Extensions;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services{


    public class RequestService: IRequestService
    {
        private readonly ApplicationDbContext _context;
        

        public  RequestService(ApplicationDbContext context){
            this._context=context;
            
        }

        public async Task<RequestResponseDto> CreateRequestAsync(RequestCreateDto requestCreateDto)
        {
            if(requestCreateDto==null)
            {
                throw new ArgumentNullException(nameof(requestCreateDto)," Cannot be null");
            }
            if (requestCreateDto.UserId<=0)
            {
                throw new ArgumentNullException(nameof(requestCreateDto.UserId)," Cannot be null");
            }
            var request =requestCreateDto.ToEntity();
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            return request.ToResponseDto();
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            if (id<=0)
            {
                throw new ArgumentException("Invalid request id ",nameof(id));
            }
            var request = FindRequestById(id);
            if(request==null)
            {
                return false;
            }
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync()
        {
            var request = await _context.Requests.ToListAsync(); 
            return request.Select(r=>r.ToResponseDto());
        }

        public async Task<RequestResponseDto?> GetRequestByIdAsync(int id)
        {
            if (id<=0)
            {
                throw new ArgumentException("Invalid request id ",nameof(id));
            }
            var request= await _context.Requests.FindAsync(id);
            if(request==null)
                throw new ArgumentException(nameof(request)," Cannot find");
            return request.ToResponseDto();
        }

        public async Task<bool> UpdateRequestAsync(int id,RequestUpdateDto requestUpdateDto)
        {
            if (requestUpdateDto==null)
            {
                return false;
            }
            var oldRequest = FindRequestById(id);
            _context.Entry(oldRequest).CurrentValues.SetValues(requestUpdateDto);
            await _context.SaveChangesAsync();
            return true;
        }
        public  Request FindRequestById(int id)
        {
            var request =_context.Requests.Find(id);
            if(request==null)
                throw new ArgumentNullException(nameof(id),"cannot find");
            return request;
        }
    }
}