using pd311_web_api.BLL.DTOs.User;

namespace pd311_web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<UserDto?> CreateAsync(UserDto dto);  // Змінено тип повернення
        Task<bool> UpdateAsync(UserDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
