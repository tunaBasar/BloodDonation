using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.DTOs.RequestDto
{
    public class RequestCreateDto
    {
        public BloodType BloodType { get; set; }
        public int UserId { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public string? Description { get; set; }
    }
}