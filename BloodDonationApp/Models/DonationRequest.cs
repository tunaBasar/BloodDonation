using BloodDonationApp.Models;

public class DonationRequest
{
    public int Id { get; set; }
    public BloodType BloodType { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public UrgencyLevel UrgencyLevel { get; set; }
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string BloodTypeDisplay => GetBloodTypeDisplayName(BloodType);
    public string UrgencyDisplay => GetUrgencyDisplayName(UrgencyLevel);
    
    private string GetBloodTypeDisplayName(BloodType bloodType)
    {
        return bloodType switch
        {
            BloodType.APositive => "A Rh+",
            BloodType.ANegative => "A Rh-",
            BloodType.BPositive => "B Rh+",
            BloodType.BNegative => "B Rh-",
            BloodType.ABPositive => "AB Rh+",
            BloodType.ABNegative => "AB Rh-",
            BloodType.OPositive => "O Rh+",
            BloodType.ONegative => "O Rh-",
            _ => bloodType.ToString()
        };
    }
    
    private string GetUrgencyDisplayName(UrgencyLevel urgency)
    {
        return urgency switch
        {
            UrgencyLevel.Low => "Düşük",
            UrgencyLevel.Medium => "Orta",
            UrgencyLevel.High => "Yüksek",
            UrgencyLevel.Critical => "Kritik",
            _ => urgency.ToString()
        };
    }
}

public enum UrgencyLevel
{
    Low,
    Medium,
    High,
    Critical
}