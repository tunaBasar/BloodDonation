namespace BloodDonationApp.Models
{
    public class RegisterDto
    {
        public string? name { get; set; }
        public string? surName { get; set; }
        public string? tc { get; set; }
        public string? phoneNumber { get; set; }
        public string? mail { get; set; }
        public string? password { get; set; }
        
        public BloodType BloodType { get; set; }
    }
}