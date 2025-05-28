using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.DTOs.UserDto
{
    public class UserCreateDto
    {
        public required string Name { get; set; }
        public required string SurName { get; set; }
        public required string Mail { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public BloodType BloodType { get; set; }
        public required string Tc { get; set; }
    }
}