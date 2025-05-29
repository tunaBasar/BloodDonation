using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.DTOs.AdminDto;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BloodDonationAPI.Entities;

namespace BloodDonationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public AuthController(IUserService userService, IAdminService adminService)
        {
            this._userService = userService;
            this._adminService = adminService;
        }

        [HttpPost("adminlogin")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto dto)
        {
            try
            {
                var result = await _adminService.AdminLoginAsync(dto);
                if (result)
                {
                    return Ok("Admin girisi basarili");
                }
                else
                {
                    return BadRequest("Admin girisi basarisiz");
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu hatası oluştu.");
            }
        }


        [HttpPost("userlogin")]
        public async Task<IResult> UserLogin([FromBody] UserLoginDto dto)
        {
            try
            {
                var user = _userService.FindByTc(dto.Tc);
                var result = await _userService.UserLoginAsync(dto);
                var userDto = new UserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    SurName = user.SurName,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                };
                return Results.Json(new Response<UserResponseDto>
                {
                    Success = result,
                    Message = result ? "Giris Basarili" : "Giris basarisiz",
                    Data = userDto
                });
            }
            catch (ArgumentNullException ex)
            {
                return Results.Json(new Response<UserResponseDto> { Success = false, Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Json(new Response<UserResponseDto> { Success = false, Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Json(new Response<UserResponseDto> { Success = false, Message = ex.Message });
            }
            catch (Exception)
            {
                return Results.Json(new Response<UserResponseDto> { Success = false, Message = "Sunucu hatası oluştu." });
            }
        }

        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] UserCreateDto dto)
        {
            try
            {
                var result = await _userService.UserRegisterAsync(dto);
                return Results.Json(new Response<UserResponseDto>
                {
                    Success = result,
                    Message = result ? "Kayit Basarili" : "Kayit basarisiz"
                });
            }
            catch (ArgumentNullException ex)
            {
                return Results.Json(new Response<UserResponseDto> { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Json(new Response<UserResponseDto> { Success = false, Message = ex.Message });
            }
        }

    }
}