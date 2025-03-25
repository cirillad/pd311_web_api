using AutoMapper;
using pd311_web_api.DTOs; // Виправлений namespace для DTO

namespace pd311_web_api.BLL.MapperProfiles
{
    public class CarAutoMapperProfile : Profile
    {
        public CarAutoMapperProfile()
        {
            // Мапінг Car <-> CarDto
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.Gearbox, opt => opt.MapFrom(src => src.Gearbox))
                .ForMember(dest => dest.ManufactureId, opt => opt.MapFrom(src => src.ManufactureId))
                .ReverseMap(); // Додано для мапінгу в обидва боки
        }
    }
}
