using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Tc { get; set; } = string.Empty;
        public int StarPoint { get; set; } = 0;
        public BloodType BloodType { get; set; }
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}

