using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.Cars;
using pd311_web_api.BLL.Services.Image;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pd311_web_api.DTOs;

namespace pd311_web_api.BLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IImageService imageService, IMapper mapper)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        // READ (Single)
        public async Task<ServiceResponse<CarDto>> GetCarByIdAsync(string id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);
                if (car == null)
                {
                    return new ServiceResponse<CarDto>("Car not found", false);
                }

                return new ServiceResponse<CarDto>("Car found", true, _mapper.Map<CarDto>(car));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CarDto>($"Error: {ex.Message}", false);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CarDto>>> GetAllCarsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = _carRepository.GetAll();

                var totalCars = await query.CountAsync();
                var cars = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                var paginationMetadata = new
                {
                    TotalCars = totalCars,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCars / pageSize)
                };

                return new ServiceResponse<IEnumerable<CarDto>>(
                    "Машини успішно отримано",
                    true,
                    carDtos
                )
                {
                    Message = "Машини успішно отримано: " + paginationMetadata.TotalCars + " записів"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CarDto>>($"Помилка: {ex.Message}", false);
            }
        }



        // CREATE
        public async Task<ServiceResponse<CarDto>> CreateCarAsync(CarDto carDto, List<IFormFile> images)
        {
            try
            {
                var car = _mapper.Map<Car>(carDto);

                // Assuming CreateAsync returns bool
                var isCreated = await _carRepository.CreateAsync(car);

                if (!isCreated)  // Direct check if created
                {
                    return new ServiceResponse<CarDto>("Failed to create car", false);
                }

                // Save images
                var imagePaths = await _imageService.SaveImagesAsync(images, $"Сars/{car.Id}");
                if (imagePaths != null && imagePaths.Any())
                {
                    // Save image paths to the database if necessary
                }

                return new ServiceResponse<CarDto>("Car created successfully", true, _mapper.Map<CarDto>(car));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CarDto>($"Error: {ex.Message}", false);
            }
        }


        public async Task<ServiceResponse<CarDto>> UpdateCarAsync(string id, CarDto carDto, List<IFormFile> images)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);
                if (car == null)
                {
                    return new ServiceResponse<CarDto>("Car not found", false);
                }

                _mapper.Map(carDto, car);

                // If UpdateAsync returns bool, check directly
                var isUpdated = await _carRepository.UpdateAsync(car);
                if (!isUpdated)  // Check if update was successful
                {
                    return new ServiceResponse<CarDto>("Failed to update car", false);
                }

                // Save images
                var imagePaths = await _imageService.SaveImagesAsync(images, $"cars/{id}");
                if (imagePaths != null && imagePaths.Any())
                {
                    // Save image paths to the database if necessary
                }

                return new ServiceResponse<CarDto>("Car updated successfully", true, _mapper.Map<CarDto>(car));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<CarDto>($"Error: {ex.Message}", false);
            }
        }


        public async Task<ServiceResponse<bool>> DeleteCarAsync(string id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);
                if (car == null)
                {
                    return new ServiceResponse<bool>("Car not found", false, false);
                }

                var success = await _carRepository.DeleteAsync(car);
                if (!success)  // If DeleteAsync returns bool, handle accordingly
                {
                    return new ServiceResponse<bool>("Failed to delete car", false, false);
                }

                return new ServiceResponse<bool>("Car deleted successfully", true, true);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>($"Error: {ex.Message}", false, false);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByBrandAsync(string brand, int pageNumber, int pageSize)
        {
            try
            {
                // Заміна Manufacturer на Brand
                var query = _carRepository.GetAll().Where(c => c.Brand == brand);

                var totalCars = await query.CountAsync();
                var cars = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                return new ServiceResponse<IEnumerable<CarDto>>("Машини бренду успішно отримано", true, carDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CarDto>>($"Помилка: {ex.Message}", false);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByYearAsync(int year, int pageNumber, int pageSize)
        {
            try
            {
                var query = _carRepository.GetAll().Where(c => c.Year == year);

                var totalCars = await query.CountAsync();
                var cars = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                return new ServiceResponse<IEnumerable<CarDto>>("Машини року випуску успішно отримано", true, carDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CarDto>>($"Помилка: {ex.Message}", false);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByGearboxAsync(string gearbox, int pageNumber, int pageSize)
        {
            try
            {
                // Заміна Transmission на Gearbox
                var query = _carRepository.GetAll().Where(c => c.Gearbox == gearbox);

                var totalCars = await query.CountAsync();
                var cars = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                return new ServiceResponse<IEnumerable<CarDto>>("Машини за типом коробки передач успішно отримано", true, carDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CarDto>>($"Помилка: {ex.Message}", false);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CarDto>>> GetCarsByColorAsync(string color, int pageNumber, int pageSize)
        {
            try
            {
                var query = _carRepository.GetAll().Where(c => c.Color == color);

                var totalCars = await query.CountAsync();
                var cars = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                return new ServiceResponse<IEnumerable<CarDto>>("Машини за кольором успішно отримано", true, carDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CarDto>>($"Помилка: {ex.Message}", false);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CarDto>>> SearchCarsByModelAsync(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                var query = _carRepository.GetAll().Where(c => c.Model.Contains(searchTerm));

                var totalCars = await query.CountAsync();
                var cars = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                return new ServiceResponse<IEnumerable<CarDto>>("Результати пошуку за моделлю успішно отримано", true, carDtos);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<CarDto>>($"Помилка: {ex.Message}", false);
            }
        }

    }
}
