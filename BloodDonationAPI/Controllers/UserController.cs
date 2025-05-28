using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService){
            this._userService=userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>>GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>>GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>>CreateUser([FromBody] UserCreateDto dto)
        {
            var createdUser= await _userService.CreateUserAsync(dto);

            return CreatedAtAction(nameof(GetUserById),
            new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult>UpdateUser([FromBody] UserUpdateDto dto,int id)
        {
            var result = await _userService.UpdateUserAsync(id,dto);
            if(!result){
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user= await _userService.DeleteUserAsync(id);
            if (!user)
                return NotFound();
            return NoContent();
        }

    }
    
}