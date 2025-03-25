using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs;
using pd311_web_api.BLL.DTOs.Manufactures;
using pd311_web_api.BLL.Services;
using System.IO;
using System.Threading.Tasks;

namespace pd311_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufactureController : ControllerBase
    {
        private readonly IManufactureService _manufactureService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManufactureController(IManufactureService manufactureService, IWebHostEnvironment webHostEnvironment)
        {
            _manufactureService = manufactureService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/manufacture/list
        [HttpGet("list")]
        public async Task<IActionResult> GetAllManufactures()
        {
            var serviceResponse = await _manufactureService.GetAllManufacturesAsync();

            if (!serviceResponse.IsSuccess)
            {
                return BadRequest(new { message = serviceResponse.Message });
            }

            return Ok(serviceResponse.Payload);  // Payload is a List<ManufactureDto>
        }

        // GET: api/manufacture/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufactureById(string id)
        {
            var serviceResponse = await _manufactureService.GetManufactureByIdAsync(id);

            if (!serviceResponse.IsSuccess)
            {
                return NotFound(new { message = serviceResponse.Message });
            }

            return Ok(serviceResponse.Payload);  // Payload is ManufactureDto
        }

        [HttpPost]
        public async Task<IActionResult> CreateManufacture([FromForm] CreateManufactureDto createManufactureDto)
        {
            if (createManufactureDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            if (createManufactureDto.Image != null)
            {
                // Збереження зображення на сервері
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", createManufactureDto.Image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createManufactureDto.Image.CopyToAsync(stream);
                }

                // Nullify the image field if you want to handle it in the service layer.
                createManufactureDto.Image = null;
            }

            // Перетворення CreateManufactureDto в ManufactureDto
            var manufactureDto = new ManufactureDto
            {
                Name = createManufactureDto.Name,
                Description = createManufactureDto.Description,
                Founder = createManufactureDto.Founder,
                Director = createManufactureDto.Director,
                // Якщо ви зберігаєте картинку в сервісі, то тут можна додати логіку для збереження шляху
                Image = createManufactureDto.Image != null ? Path.Combine("images", createManufactureDto.Image.FileName) : null
            };

            var serviceResponse = await _manufactureService.CreateManufactureAsync(manufactureDto);

            if (!serviceResponse.IsSuccess)
            {
                return BadRequest(new { message = serviceResponse.Message });
            }

            return CreatedAtAction(nameof(GetManufactureById), new { id = serviceResponse.Payload?.Id }, serviceResponse.Payload);
        }

        // PUT: api/manufacture/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManufacture(string id, [FromForm] UpdateManufactureDto updateManufactureDto)
        {
            if (updateManufactureDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            if (updateManufactureDto.Image != null)
            {
                // Збереження зображення на сервері
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", updateManufactureDto.Image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateManufactureDto.Image.CopyToAsync(stream);
                }

                // Nullify the image field if you want to handle it in the service layer.
                updateManufactureDto.Image = null;
            }

            // Перетворення UpdateManufactureDto в ManufactureDto
            var manufactureDto = new ManufactureDto
            {
                Id = id,  // Id передається як параметр в URL
                Name = updateManufactureDto.Name,
                Description = updateManufactureDto.Description,
                Founder = updateManufactureDto.Founder,
                Director = updateManufactureDto.Director,
                // Якщо ви зберігаєте картинку в сервісі, то тут можна додати логіку для збереження шляху
                Image = updateManufactureDto.Image != null ? Path.Combine("images", updateManufactureDto.Image.FileName) : null
            };

            var serviceResponse = await _manufactureService.UpdateManufactureAsync(id, manufactureDto);

            if (!serviceResponse.IsSuccess)
            {
                return NotFound(new { message = serviceResponse.Message });
            }

            return Ok(serviceResponse.Payload);  // Payload - оновлений ManufactureDto
        }

        // DELETE: api/manufacture/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacture(string id)
        {
            var serviceResponse = await _manufactureService.DeleteManufactureAsync(id);

            if (!serviceResponse.IsSuccess)
            {
                return NotFound(new { message = serviceResponse.Message });
            }

            return NoContent(); // 204 No Content - successful deletion
        }
    }
}
