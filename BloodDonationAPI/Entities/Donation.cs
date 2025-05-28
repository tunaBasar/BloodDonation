namespace BloodDonationAPI.Entities
{
    public class Donation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RequestId { get; set; }

        public User User { get; set; } = null!;
        public Request Request { get; set; } = null!;
    }

}
