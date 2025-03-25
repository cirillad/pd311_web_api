using Microsoft.AspNetCore.Mvc;
using pd311_web_api.DTOs;
using pd311_web_api.BLL.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pd311_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        // POST: api/cars
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CarDto carDto, [FromForm] List<IFormFile> images)
        {
            var response = await _carService.CreateCarAsync(carDto, images);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(string id)
        {
            var response = await _carService.GetCarByIdAsync(id);
            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars
        [HttpGet]
        public async Task<IActionResult> GetAllCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _carService.GetAllCarsAsync(pageNumber, pageSize);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars/brand/{brand}
        [HttpGet("brand/{brand}")]
        public async Task<IActionResult> GetCarsByBrand(string brand, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _carService.GetCarsByBrandAsync(brand, pageNumber, pageSize);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars/year/{year}
        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetCarsByYear(int year, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _carService.GetCarsByYearAsync(year, pageNumber, pageSize);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars/gearbox/{gearbox}
        [HttpGet("gearbox/{gearbox}")]
        public async Task<IActionResult> GetCarsByGearbox(string gearbox, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _carService.GetCarsByGearboxAsync(gearbox, pageNumber, pageSize);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars/color/{color}
        [HttpGet("color/{color}")]
        public async Task<IActionResult> GetCarsByColor(string color, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _carService.GetCarsByColorAsync(color, pageNumber, pageSize);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // GET: api/cars/search/{searchTerm}
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchCarsByModel(string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _carService.SearchCarsByModelAsync(searchTerm, pageNumber, pageSize);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // PUT: api/cars/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(string id, [FromBody] CarDto carDto, [FromForm] List<IFormFile> images)
        {
            var response = await _carService.UpdateCarAsync(id, carDto, images);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }

        // DELETE: api/cars/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(string id)
        {
            var response = await _carService.DeleteCarAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Payload);
        }
    }
}
