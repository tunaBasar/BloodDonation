using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.DTOs.RequestDto
{
    public class RequestResponseDto
    {
        public int Id { get; set; }
        public BloodType BloodType { get; set; }
        public int UserId { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        
    }
}