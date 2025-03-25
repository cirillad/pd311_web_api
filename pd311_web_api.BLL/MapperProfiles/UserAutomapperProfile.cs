using AutoMapper;
using pd311_web_api.BLL.DTOs.User;
using pd311_web_api.BLL.DTOs.Role;
using static pd311_web_api.DAL.Entities.IdentityEntities;
using pd311_web_api.BLL.DTOs.Account;

namespace pd311_web_api.BLL.MapperProfiles
{
    public class UserAutomapperProfile : Profile
    {
        public UserAutomapperProfile()
        {
            // RegisterDto -> AppUser mapping
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Password will be handled separately
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());  // Ignore UserRoles, roles will be set later

            // AppUser -> UserDto mapping
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => new RoleDto { Id = ur.RoleId, Name = ur.Role.Name }).ToList()));

            // RoleDto -> AppRole mapping
            CreateMap<RoleDto, AppRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));  // Assuming RoleDto has Name property

            // AppRole -> RoleDto mapping
            CreateMap<AppRole, RoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
