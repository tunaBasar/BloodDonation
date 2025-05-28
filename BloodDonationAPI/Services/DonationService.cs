using BloodDonationAPI.DataAccess;
using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Extensions;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BloodDonationAPI.Services
{
    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRequestService _request;

        private readonly IUserService _user;
        public DonationService(ApplicationDbContext context, IRequestService request,IUserService user)
        {
            this._context=context;
            this._request=request;
            this._user=user;
        }
        public async Task<DonationResponseDto> CreateDonationAsync(DonationCreateDto dto)
        {
            if(dto==null)
            {
                throw new ("For adding donation you must give me donation!!!");
            }
            var request = _request.GetRequestByIdAsync(dto.RequestId);
            var user = _user.GetUserByIdAsync(dto.UserId);
            if (request==null || user == null)
            {
                throw new ArgumentNullException("user or request cant be null!!!");
            }
            var dontaion= dto.ToEntity();
            _context.Donations.Add(dontaion);
            await _context.SaveChangesAsync();
            return dontaion.ToResponseDto();
        }

        public async Task<bool> DeleteDonationAsync(int id)
        {
            if(id<=0)
            {
                return false;
            }
            var donation = FindDontaionById(id);
            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DonationResponseDto>> GetAllDonationsAsync()
        {
            var donataion = await _context.Donations.ToListAsync();
            return donataion.Select(d=>d.ToResponseDto());
        }

        public async Task<DonationResponseDto?> GetDonationByIdAsync(int id)
        {
            if(id<=0)
            {
                throw new Exception("invalid id");
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation==null)
            {
                throw new ArgumentNullException(nameof(id)," does not match any donation");
            }
            return donation.ToResponseDto();
        }

        public async Task<bool> UpdateDonationAsync(DonationUpdateDto dto, int id)
        {
            if(id<=0)
            {
                return false;
            }
            var donation=FindDontaionById(id);
            _context.Entry(donation).CurrentValues.SetValues(dto);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public Donation FindDontaionById(int id)
        {
            if(id<=0)
            {
                throw new ArgumentException(nameof(id)," does not avaible");
            }
            var donation=_context.Donations.Find(id);
            if(donation==null)
            {
                throw new ArgumentNullException(nameof(id)," does not match any donation");
            }
            return donation;
        }
    }
}