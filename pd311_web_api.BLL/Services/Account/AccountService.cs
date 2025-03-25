using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Email;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService, IMapper mapper, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<bool>> ConfirmEmailAsync(string id, string base64)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var bytes = Convert.FromBase64String(base64);
                var token = Encoding.UTF8.GetString(bytes);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                return new ServiceResponse<bool>(result.Succeeded ? "Email підтверджено успішно" : "Не вдалося підтвердити email", result.Succeeded);
            }

            return new ServiceResponse<bool>("Користувача не знайдено", false);
        }

        public async Task<ServiceResponse<AppUser?>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName ?? "");

            if (user == null)
                return new ServiceResponse<AppUser?>($"Користувача з іменем '{dto.UserName}' не знайдено");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password ?? "");

            if (!result)
                return new ServiceResponse<AppUser?>($"Пароль вказано невірно");

            var jwtToken = GenerateJwtToken(user);

            return new ServiceResponse<AppUser?>("Успішний вхід", true, user, jwtToken);
        }

        public async Task<ServiceResponse<AppUser?>> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse<AppUser?>($"Email '{dto.Email}' зайнятий");

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse<AppUser?>($"Ім'я '{dto.UserName}' вже використовується");

            var user = _mapper.Map<AppUser>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return new ServiceResponse<AppUser?>(result.Errors.First().Description);

            if (result.Succeeded && await _roleManager.RoleExistsAsync("user"))
                await _userManager.AddToRoleAsync(user, "user");

            await SendConfirmEmailTokenAsync(user.Id);

            return new ServiceResponse<AppUser?>("Успішна реєстрація", true, user);
        }

        public async Task<ServiceResponse<bool>> SendConfirmEmailTokenAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return new ServiceResponse<bool>("Користувача не знайдено", false, false);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

                var body = $"<a href='https://localhost:7223/api/account/confirmEmail?id={user.Id}&t={base64Token}'>Підтвердити пошту</a>";

                await _emailService.SendMailAsync(user.Email!, "Email confirm", body, true);

                return new ServiceResponse<bool>("Лист із підтвердженням надіслано", true, true);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>($"Помилка: {ex.Message}", false, false);
            }
        }

        public async Task<ServiceResponse<List<AppUser>>> GetUsersByRoleAsync(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                return new ServiceResponse<List<AppUser>>($"Роль '{role}' не знайдено", false, new List<AppUser>());

            var users = await _userManager.GetUsersInRoleAsync(role);
            return new ServiceResponse<List<AppUser>>("Користувачі знайдені", true, users.ToList());
        }

        public async Task<ServiceResponse<List<AppUser>>> GetSortedUsersAsync(string sortBy)
        {
            var users = _userManager.Users.AsQueryable();

            users = sortBy.ToLower() switch
            {
                "role" => users.OrderBy(u => _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? ""),
                "email" => users.OrderBy(u => u.Email),
                "username" => users.OrderBy(u => u.UserName),
                _ => users.OrderBy(u => u.Id) // За замовчуванням сортуємо по Id
            };

            return new ServiceResponse<List<AppUser>>("Користувачі відсортовані", true, users.ToList());
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? ""),
                new Claim("image", user.Image ?? ""),
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("lastName", user.LastName ?? "")
            };

            var roles = _userManager.GetRolesAsync(user).Result;
            if (roles.Any())
            {
                claims.AddRange(roles.Select(r => new Claim("role", r)));
            }

            string secretKey = _configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JwtSettings:Key");
            string issuer = _configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer");
            string audience = _configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience");
            int expMinutes = int.Parse(_configuration["JwtSettings:ExpTime"] ?? "60");

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
