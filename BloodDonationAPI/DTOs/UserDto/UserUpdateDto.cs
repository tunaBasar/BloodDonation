using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.DTOs.UserDto
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Mail { get; set; }
        public string? PhoneNumber { get; set; }
        public BloodType BloodType { get; set; }
        public string? Password { get; set; }
    }
}