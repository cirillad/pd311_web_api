using pd311_web_api.BLL.DTOs.Manufactures;

namespace pd311_web_api.BLL.Services
{
    public interface IManufactureService
    {
        Task<ServiceResponse<ManufactureDto>> CreateManufactureAsync(ManufactureDto manufactureDto);
        Task<ServiceResponse<ManufactureDto>> GetManufactureByIdAsync(string id);
        Task<ServiceResponse<List<ManufactureDto>>> GetAllManufacturesAsync();
        Task<ServiceResponse<ManufactureDto>> UpdateManufactureAsync(string id, ManufactureDto manufactureDto);
        Task<ServiceResponse<ManufactureDto>> DeleteManufactureAsync(string id);
    }
}
