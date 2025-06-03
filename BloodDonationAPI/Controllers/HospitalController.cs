using BloodDonationAPI.DTOs.PlacesDto;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospitalController : ControllerBase
{
    private readonly IHospitalService hospitalService;

    public HospitalController(IHospitalService hospitalService)
    {
        this.hospitalService = hospitalService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HospitalsDto>>> GetAllHospitals()
    {
        var hospitals = await hospitalService.GetAllPlacesAsync();
        return Ok(hospitals);
    }


}