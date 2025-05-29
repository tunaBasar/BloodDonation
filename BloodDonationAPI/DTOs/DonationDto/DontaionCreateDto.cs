namespace BloodDonationAPI.DTOs.DonationDto
{
    public class DonationCreateDto
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }

        public bool IsActive { get; set; }
    }
}