using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using pd311_web_api.DTOs;

namespace pd311_web_api.BLL.Services
{
    public interface ICarService
    {
        Task<ServiceResponse<CarDto>> CreateCarAsync(CarDto carDto, List<IFormFile> images);
        Task<ServiceResponse<CarDto>> GetCarByIdAsync(string id);
        Task<ServiceResponse<IEnumerable<CarDto>>> GetAllCarsAsync(int pageNumber, int pageSize);
        Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByBrandAsync(string manufacturer, int pageNumber, int pageSize);
        Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByYearAsync(int year, int pageNumber, int pageSize);
        Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByGearboxAsync(string transmission, int pageNumber, int pageSize);
        Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByColorAsync(string color, int pageNumber, int pageSize);
        Task<ServiceResponse<IEnumerable<CarDto>>> SearchCarsByModelAsync(string searchTerm, int pageNumber, int pageSize);
        Task<ServiceResponse<CarDto>> UpdateCarAsync(string id, CarDto carDto, List<IFormFile> images);
        Task<ServiceResponse<bool>> DeleteCarAsync(string id);
    }
}
