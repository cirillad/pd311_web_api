using AutoMapper;
using pd311_web_api.DAL.Entities;
using pd311_web_api.BLL.DTOs.Manufactures;

namespace pd311_web_api.BLL.MapperProfiles
{
    public class ManufactureAutomapperProfile : Profile
    {
        public ManufactureAutomapperProfile()
        {
            // Manufacture -> ManufactureDto mapping
            CreateMap<Manufacture, ManufactureDto>();

            // ManufactureDto -> Manufacture mapping
            CreateMap<ManufactureDto, Manufacture>();
        }
    }
}
