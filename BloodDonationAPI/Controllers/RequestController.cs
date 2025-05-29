using Microsoft.AspNetCore.Mvc;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Services.Interfaces;
using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.DTOs.DonationDto;

namespace BloodDonationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            this._requestService = requestService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<IEnumerable<RequestResponseDto>>>> GetAllRequests(int id)
        {
            try
            {
                var result = await _requestService.GetAllRequestsAsync(id);

                if (result != null)
                {
                    return Ok(new Response<IEnumerable<RequestResponseDto>>
                    {
                        Success = true,
                        Message = "İstekler başarıyla getirildi",
                        Data = result
                    });
                }
                else
                {
                    return Ok(new Response<IEnumerable<RequestResponseDto>>
                    {
                        Success = true,
                        Message = "Henüz istek bulunmuyor",
                        Data = new List<RequestResponseDto>()
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<IEnumerable<RequestResponseDto>>
                {
                    Success = false,
                    Message = $"Hata oluştu: {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpGet("one/{id}")]
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
            new { id = _request.Id }, _request);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> DeactiveRequest(int id, [FromBody] DeactiveRequestDto dto)
        {
            bool result = await _requestService.DeactiveRequest(id, dto);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateRequest([FromBody] RequestUpdateDto requestUpdateDto, int id)
        {
            var updatedRequest = await _requestService.UpdateRequestAsync(id, requestUpdateDto);
            if (!updatedRequest)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRequest(int id)
        {
            var deletedRequest = await _requestService.DeleteRequestAsync(id);
            if (!deletedRequest)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}