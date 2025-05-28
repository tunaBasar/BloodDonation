using BloodDonationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.DTOs.PlacesDto;

namespace BloodDonationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await adminService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("user")]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserCreateDto dto)
        {
            var createdUser = await adminService.CreateUserAsync(dto);

            return CreatedAtAction(nameof(GetUserById),
            new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("user/{id}")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto dto, int id)
        {
            var result = await adminService.UpdateUserAsync(id, dto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await adminService.DeleteUserAsync(id);
            if (!user)
                return NotFound();
            return NoContent();
        }

        [HttpGet("request")]
        public async Task<ActionResult<IEnumerable<RequestResponseDto>>> GetAllRequests()
        {
            var request = await adminService.GetAllRequestsAsync();
            return Ok(request);
        }



        [HttpGet("request/{id}")]
        public async Task<ActionResult<RequestResponseDto>> GetRequestById(int id)
        {
            var request = await adminService.GetRequestByIdAsync(id);
            return Ok(request);
        }


        [HttpPost("request")]
        public async Task<ActionResult<RequestResponseDto>> CreateRequest([FromBody] RequestCreateDto dto)
        {
            var _request = await adminService.CreateRequestAsync(dto);
            return CreatedAtAction(nameof(GetRequestById),
            new { id = _request.Id }, _request);
        }

        [HttpPut("request/{id}")]
        public async Task<ActionResult> UpdateRequest([FromBody] RequestUpdateDto requestUpdateDto, int id)
        {
            var updatedRequest = await adminService.UpdateRequestAsync(id, requestUpdateDto);
            if (!updatedRequest)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("request/{id}")]
        public async Task<ActionResult> DeleteRequest(int id)
        {
            var deletedRequest = await adminService.DeleteRequestAsync(id);
            if (!deletedRequest)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("donations")]
        public async Task<ActionResult<IEnumerable<DonationResponseDto>>> GetAllDonations()
        {
            var donation = await adminService.GetAllDonationsAsync();
            return Ok(donation);

        }

        [HttpGet("donations/{id}")]
        public async Task<ActionResult<DonationResponseDto>> GetDonationById(int id)
        {
            var donation = await adminService.GetDonationByIdAsync(id);
            return Ok(donation);
        }

        [HttpPost("donations")]
        public async Task<ActionResult<DonationResponseDto>> CreateDonation([FromBody] DonationCreateDto dto)
        {
            try
            {
                var donation = await adminService.CreateDonationAsync(dto);
                return CreatedAtAction(nameof(GetDonationById),
                new { id = donation.Id }, donation);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("donations/{id}")]
        public async Task<ActionResult> UpdateDonation(int id, [FromBody] DonationUpdateDto dto)
        {
            var donation = await adminService.UpdateDonationAsync(dto, id);
            if (donation || id <= 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("donations/{id}")]
        public async Task<ActionResult> DeleteDonation(int id)
        {
            var donation = await adminService.DeleteDonationAsync(id);
            if (id <= 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("places")]
        public async Task<ActionResult<IEnumerable<PlacesDto>>> GetAllPlace()
        {
           var place= await adminService.GetAllPlacesAsync();
           return Ok(place);
        }

        [HttpGet("places/{id}")]
        public async Task<ActionResult<PlacesDto>> GetPlaceById(int id)
        {
            var place= await adminService.GetPlacesByIdAsync(id);
            return Ok(place);
        }

        [HttpPost("places")]
        public async Task<ActionResult<PlacesDto>> CreatePlace([FromBody] PlacesDto dto)
        {
            try
            {
                var place = await adminService.CreatePlacesAsync(dto);
                return CreatedAtAction(nameof(GetPlaceById),
                new { id = place.Id }, place);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("places/{id}")]
        public async Task<ActionResult> UpdatePlace([FromBody] PlacesDto dto, int id)
        {
            var place = await adminService.UpdatePlacesAsync(id,dto);
            if (place || id <= 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("places/{id}")]
        public async Task<ActionResult> DeletePlace(int id)
        {
            var place = await adminService.DeletePlacesAsync(id);
            if (place || id <= 0)
            {
                return NotFound();
            }
            return NoContent();
        }
        
    }
}