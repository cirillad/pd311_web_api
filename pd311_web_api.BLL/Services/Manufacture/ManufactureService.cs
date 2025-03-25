using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs;
using pd311_web_api.BLL.DTOs.Manufactures;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.Manufactures;

namespace pd311_web_api.BLL.Services
{
    public class ManufactureService : IManufactureService
    {
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IMapper _mapper;

        public ManufactureService(IManufactureRepository manufactureRepository, IMapper mapper)
        {
            _manufactureRepository = manufactureRepository;
            _mapper = mapper;
        }

        // CREATE
        public async Task<ServiceResponse<ManufactureDto>> CreateManufactureAsync(ManufactureDto manufactureDto)
        {
            try
            {
                var manufacture = _mapper.Map<Manufacture>(manufactureDto);
                var success = await _manufactureRepository.CreateAsync(manufacture);
                if (!success)
                {
                    return new ServiceResponse<ManufactureDto>("Failed to create manufacture.", false);
                }

                return new ServiceResponse<ManufactureDto>("Manufacture created successfully.", true, _mapper.Map<ManufactureDto>(manufacture));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufactureDto>($"Error: {ex.Message}", false);
            }
        }

        // READ (Single)
        public async Task<ServiceResponse<ManufactureDto>> GetManufactureByIdAsync(string id)
        {
            try
            {
                var manufacture = await _manufactureRepository.GetByIdAsync(id);

                if (manufacture == null)
                    return new ServiceResponse<ManufactureDto>("Manufacture not found.", false);

                return new ServiceResponse<ManufactureDto>("Manufacture retrieved successfully.", true, _mapper.Map<ManufactureDto>(manufacture));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufactureDto>($"Error: {ex.Message}", false);
            }
        }

        // READ (All)
        public async Task<ServiceResponse<List<ManufactureDto>>> GetAllManufacturesAsync()
        {
            try
            {
                var manufactures = await _manufactureRepository.GetAll().ToListAsync();
                return new ServiceResponse<List<ManufactureDto>>("Manufactures retrieved successfully.", true, _mapper.Map<List<ManufactureDto>>(manufactures));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<ManufactureDto>>($"Error: {ex.Message}", false);
            }
        }

        // UPDATE
        public async Task<ServiceResponse<ManufactureDto>> UpdateManufactureAsync(string id, ManufactureDto manufactureDto)
        {
            try
            {
                // Отримуємо виробника за ID
                var manufacture = await _manufactureRepository.GetByIdAsync(id);
                if (manufacture == null)
                    return new ServiceResponse<ManufactureDto>("Manufacture not found.", false);

                // Оновлюємо інформацію
                _mapper.Map(manufactureDto, manufacture);
                var success = await _manufactureRepository.UpdateAsync(manufacture);

                if (!success)
                {
                    return new ServiceResponse<ManufactureDto>("Failed to update manufacture.", false);
                }

                return new ServiceResponse<ManufactureDto>("Manufacture updated successfully.", true, _mapper.Map<ManufactureDto>(manufacture));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufactureDto>($"Error: {ex.Message}", false);
            }
        }

        // DELETE
        public async Task<ServiceResponse<ManufactureDto>> DeleteManufactureAsync(string id)
        {
            try
            {
                var manufacture = await _manufactureRepository.GetByIdAsync(id);
                if (manufacture == null)
                    return new ServiceResponse<ManufactureDto>("Manufacture not found.", false);

                var success = await _manufactureRepository.DeleteAsync(manufacture);

                if (!success)
                {
                    return new ServiceResponse<ManufactureDto>("Failed to delete manufacture.", false);
                }

                return new ServiceResponse<ManufactureDto>("Manufacture deleted successfully.", true, _mapper.Map<ManufactureDto>(manufacture));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufactureDto>($"Error: {ex.Message}", false);
            }
        }
    }
}
