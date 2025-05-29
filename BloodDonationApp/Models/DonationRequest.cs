namespace BloodDonationApp.Models
{
    public class DonationRequest
    {
        public int Id { get; set; }
        public string? BloodType { get; set; }
        public string? PatientName { get; set; }
        public string? HospitalName { get; set; }
        public string? UrgencyLevel { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
    }
}