using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.User;
using pd311_web_api.BLL.Services.User;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDto> _userValidator;

        public UserController(IUserService userService, IValidator<UserDto> userValidator)
        {
            _userService = userService;
            _userValidator = userValidator;
        }

        // Get all users
        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // Get user by Id
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            if (!ValidateId(id, out string message))
                return BadRequest(message);

            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID cannot be empty.");

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // Create user
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserDto userDto)
        {

            // Validate the input data
            if (string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Password is required.");
            }

            var validationResult = await _userValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userService.CreateAsync(userDto);
            if (user != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
            }

            return BadRequest("User creation failed.");
        }

        // Update user
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto userDto)
        {
            // Validate userDto and check ID validity
            if (string.IsNullOrEmpty(userDto.Id))
                return BadRequest("User ID is required.");

            var result = await _userService.UpdateAsync(userDto);
            return result ? Ok("User updated successfully") : BadRequest("User update failed.");
        }

        // Delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID cannot be empty.");

            var result = await _userService.DeleteAsync(id);
            return result ? Ok("User deleted successfully") : BadRequest("User deletion failed.");
        }
    }
}
