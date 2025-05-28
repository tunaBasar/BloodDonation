using BloodDonationAPI.Entities.Enums;

namespace BloodDonationAPI.Entities
{
    public class Request
{
    public int Id { get; set; }
    public BloodType BloodType { get; set; }
    public int UserId { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
    public string? Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}

}
