using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Account;
using pd311_web_api.BLL.Services;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<LoginDto> _loginValidator;

        public AccountController(IAccountService accountService, IValidator<LoginDto> loginValidator)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            // validation >>>
            var validationResult = await _loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);
            // <<< end

            var result = await _accountService.LoginAsync(dto);

            if (result == null)
                return BadRequest("Incorrect userName or password");

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var response = await _accountService.RegisterAsync(dto);

            if (response == null)
                return BadRequest("Registration failed");

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response.Payload); // Return the registered user
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string? id, string? t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t))
                return NotFound();

            var response = await _accountService.ConfirmEmailAsync(id, t);

            if (response.IsSuccess)
                return Redirect("https://google.com"); // Redirect after confirmation
            else
                return BadRequest(response.Message);
        }


        [HttpGet("sendConfirmEmailToken")]
        public async Task<IActionResult> SendConfirmEmailTokenAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var response = await _accountService.SendConfirmEmailTokenAsync(userId);

            if (response.IsSuccess)
                return Ok("Email sent");
            else
                return BadRequest(response.Message);
        }

        [HttpGet("usersByRole")]
        public async Task<IActionResult> GetUsersByRoleAsync([FromQuery] string role)
        {
            var response = await _accountService.GetUsersByRoleAsync(role);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response.Payload);
        }

        [HttpGet("sortedUsers")]
        public async Task<IActionResult> GetSortedUsersAsync([FromQuery] string sortBy)
        {
            var response = await _accountService.GetSortedUsersAsync(sortBy);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response.Payload);
        }


    }
}
