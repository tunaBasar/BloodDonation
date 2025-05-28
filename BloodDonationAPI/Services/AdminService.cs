using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.Services.Interfaces;
using BloodDonationAPI.DTOs.AdminDto;
using BloodDonationAPI.DTOs.PlacesDto;
using BloodDonationAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDonationService _donation;
        private readonly IUserService _user;
        private readonly IRequestService _request;
        private readonly IPlacesService _places;

        public AdminService(IDonationService donation, IUserService user, IRequestService request, ApplicationDbContext context, IPlacesService places)
        {
            _context = context;
            _donation = donation;
            _request = request;
            _user = user;
            _places = places;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto dto)
        {
            return await _user.CreateUserAsync(dto);
        }
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            return await _user.UpdateUserAsync(id, dto);
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _user.DeleteUserAsync(id);
        }
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _user.GetAllUsersAsync();
        }
        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            return await _user.GetUserByIdAsync(id);
        }

        public async Task<DonationResponseDto> CreateDonationAsync(DonationCreateDto dto)
        {
            return await _donation.CreateDonationAsync(dto);
        }
        public async Task<bool> DeleteDonationAsync(int id)
        {
            return await _donation.DeleteDonationAsync(id);
        }
        public async Task<bool> UpdateDonationAsync(DonationUpdateDto dto, int id)
        {
            return await _donation.UpdateDonationAsync(dto, id);
        }
        public async Task<DonationResponseDto?> GetDonationByIdAsync(int id)
        {
            return await _donation.GetDonationByIdAsync(id);
        }
        public async Task<IEnumerable<DonationResponseDto>> GetAllDonationsAsync()
        {
            return await _donation.GetAllDonationsAsync();
        }

        public async Task<RequestResponseDto> CreateRequestAsync(RequestCreateDto requestCreateDto)
        {
            return await _request.CreateRequestAsync(requestCreateDto);
        }
        public async Task<bool> DeleteRequestAsync(int id)
        {
            return await _request.DeleteRequestAsync(id);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync()
        {
            return await _request.GetAllRequestsAsync();
        }
        public async Task<RequestResponseDto?> GetRequestByIdAsync(int id)
        {
            return await _request.GetRequestByIdAsync(id);
        }
        public async Task<bool> UpdateRequestAsync(int id, RequestUpdateDto requestUpdateDto)
        {
            return await _request.UpdateRequestAsync(id, requestUpdateDto);
        }

        public async Task<PlacesDto> CreatePlacesAsync(PlacesDto dto)
        {
            return await _places.CreatePlacesAsync(dto);
        }
        public async Task<PlacesDto?> GetPlacesByIdAsync(int id)
        {
            return await _places.GetPlacesByIdAsync(id);
        }
        public async Task<IEnumerable<PlacesDto>> GetAllPlacesAsync()
        {
            return await _places.GetAllPlacesAsync();
        }
        public async Task<bool> UpdatePlacesAsync(int id, PlacesDto dto)
        {
            return await _places.UpdatePlacesAsync(id,dto);
        }
        public async Task<bool> DeletePlacesAsync(int id)
        {
            return await _places.DeletePlacesAsync(id);
        }

        public async Task<bool> AdminLoginAsync(AdminLoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.AdminUserName) || string.IsNullOrEmpty(dto.Password))
                throw new ArgumentNullException("User_name veya şifre boş olamaz");

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminUserName == dto.AdminUserName);

            if (admin == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı");
            if (admin.Password != dto.Password)
                throw new UnauthorizedAccessException("KullanıcıAdı veya Şifre yanlış");

            return true;
        }

    }

}