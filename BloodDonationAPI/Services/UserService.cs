using BloodDonationAPI.Entities;
using BloodDonationAPI.Services.Interfaces;
using BloodDonationAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BloodDonationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), " Cannot be null");
            }
            var user = dto.ToEntity();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.ToResponseDto();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            var user = FindUserById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var user = await _context.Users.ToListAsync();
            var userDtos = user.Select(u => u.ToResponseDto());
            return userDtos;
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), " does not contain");
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(id), " does not match any user!!");
            }
            var userDtos = user.ToResponseDto();
            return userDtos;
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            if (dto == null)
            {
                return false;
            }
            var oldUser = FindUserById(id);
            _context.Entry(oldUser).CurrentValues.SetValues(dto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserLoginAsync(UserLoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Tc) || string.IsNullOrEmpty(dto.Password))
                throw new ArgumentNullException("Tc veya şifre boş olamaz");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Tc == dto.Tc);
            if (user == null)
                throw new UnauthorizedAccessException("Kullanıcı bulunamadı");

            if (user.Password != dto.Password)
                throw new InvalidOperationException("Şifre yanlış veya Tc yanlış");

            return true;
        }

        public async Task<bool> UserRegisterAsync(UserCreateDto dto)
        {
            if (string.IsNullOrEmpty(dto.Mail) ||
                string.IsNullOrEmpty(dto.Password) ||
                string.IsNullOrEmpty(dto.Name) ||
                string.IsNullOrEmpty(dto.SurName) ||
                string.IsNullOrEmpty(dto.Tc) ||
                string.IsNullOrEmpty(dto.PhoneNumber))
            {
                throw new ArgumentNullException("Alan boş olamaz");
            }

            var userExist = await _context.Users.AnyAsync(u => u.Tc == dto.Tc);
            if (userExist)
            {
                throw new Exception("Bu Tc sistemde kayitli");
            }
            else
            {
                var registeredUser = await CreateUserAsync(dto);
                return registeredUser != null;
            }
        }

        public User FindUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(id), " this id is not avaible");
            }
            return user;
        }

        public User FindByTc(string tc)
        {
            User user = _context.Users.FirstOrDefault(u => u.Tc == tc);
            return user;
        }

    }
}
