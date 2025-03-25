using pd311_web_api.BLL.DTOs.Account;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        public Task<ServiceResponse<AppUser?>> LoginAsync(LoginDto dto);
        public Task<ServiceResponse<AppUser?>> RegisterAsync(RegisterDto dto);
        public Task<ServiceResponse<bool>> ConfirmEmailAsync(string id, string token);
        public Task<ServiceResponse<bool>> SendConfirmEmailTokenAsync(string userId);
        public Task<ServiceResponse<List<AppUser>>> GetUsersByRoleAsync(string role);
        public Task<ServiceResponse<List<AppUser>>> GetSortedUsersAsync(string sortBy);
    }
}
