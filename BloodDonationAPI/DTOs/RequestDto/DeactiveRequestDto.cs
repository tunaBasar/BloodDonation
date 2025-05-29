namespace BloodDonationAPI.DTOs.RequestDto;

public class DeactiveRequestDto
{
    public int requestId{ get; set; }
    public int userId { get; set; }

    public bool IsActive { get; set; }
}