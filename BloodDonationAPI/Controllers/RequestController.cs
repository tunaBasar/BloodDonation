using Microsoft.AspNetCore.Mvc;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Services.Interfaces;
using BloodDonationAPI.DTOs.RequestDto;

namespace BloodDonationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            this._requestService=requestService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestResponseDto>>>GetAllRequests()
        {
            var request =await _requestService.GetAllRequestsAsync();
            return Ok(request); 
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestResponseDto>> GetRequestById(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<RequestResponseDto>> CreateRequest([FromBody] RequestCreateDto requestCreateDto)
        {
            var _request = await _requestService.CreateRequestAsync(requestCreateDto);
            return CreatedAtAction(nameof(GetRequestById),
            new {id=_request.Id},_request);

        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRequest([FromBody] RequestUpdateDto requestUpdateDto , int id)
        {
            var updatedRequest= await _requestService.UpdateRequestAsync(id,requestUpdateDto);
            if(!updatedRequest)
            {
                return NotFound();
            }
            return NoContent();
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRequest(int id)
        {
            var deletedRequest= await _requestService.DeleteRequestAsync(id);
            if(!deletedRequest)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}