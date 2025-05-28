using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService donationService)
        {
            this._donationService = donationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationResponseDto>>> GetAllDonations()
        {
            var donation = await _donationService.GetAllDonationsAsync();
            return Ok(donation);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DonationResponseDto>> GetDonationById(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            return Ok(donation);
        }

        [HttpPost]
        public async Task<ActionResult<DonationResponseDto>> CreateDonation([FromBody] DonationCreateDto dto)
        {
            try
            {
                var donation = await _donationService.CreateDonationAsync(dto);
                return CreatedAtAction(nameof(GetDonationById),
                new { id = donation.Id }, donation);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDonation(int id)
        {
            var donation = await _donationService.DeleteDonationAsync(id);
            if (id <= 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDonation(int id, [FromBody] DonationUpdateDto dto)
        {
            var donation = await _donationService.UpdateDonationAsync(dto, id);
            if (dto == null || id <= 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}