using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.User;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        // Get all users
        public async Task<List<UserDto>> GetAllAsync()
        {
            var entities = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();

            var dtos = _mapper.Map<List<UserDto>>(entities);

            return dtos;
        }

        // Get user by Id
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<UserDto>(entity);

            return dto;
        }

        // Create a new user
        public async Task<UserDto?> CreateAsync(UserDto dto)
        {
            // Validate password
            if (string.IsNullOrEmpty(dto.Password))
                return null;

            // Map UserDto to AppUser entity
            var entity = _mapper.Map<AppUser>(dto);

            // Create the user in the database
            var result = await _userManager.CreateAsync(entity, dto.Password);

            if (result.Succeeded)
            {
                if (dto.Roles != null && dto.Roles.Any())
                {
                    foreach (var role in dto.Roles)
                    {
                        await _userManager.AddToRoleAsync(entity, role.Name);
                    }
                }

                return _mapper.Map<UserDto>(entity);
            }

            return null;
        }




        // Update user information
        public async Task<bool> UpdateAsync(UserDto dto)
        {
            var entity = await _userManager.FindByIdAsync(dto.Id);

            if (entity == null)
                return false;

            entity.UserName = dto.UserName;
            entity.Email = dto.Email;
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;

            var result = await _userManager.UpdateAsync(entity);

            return result.Succeeded;
        }

        // Delete user by Id
        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);

            if (entity != null)
            {
                var result = await _userManager.DeleteAsync(entity);
                return result.Succeeded;
            }

            return false;
        }
    }
}
