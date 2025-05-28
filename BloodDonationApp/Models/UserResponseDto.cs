namespace BloodDonationApp.Models
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Mail { get; set; }
        public string? PhoneNumber { get; set; }
        public BloodType BloodType { get; set; }
        public int StarPoint { get; set; }
    }
    public enum BloodType
    {
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative
    }

}
