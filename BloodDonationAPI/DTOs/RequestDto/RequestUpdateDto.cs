using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.DTOs.RequestDto
{
    public class RequestUpdateDto
    {
        public UrgencyLevel UrgencyLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}