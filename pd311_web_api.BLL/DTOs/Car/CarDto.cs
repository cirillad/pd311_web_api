using System.ComponentModel.DataAnnotations;

namespace pd311_web_api.DTOs
{
    public class CarDto
    {
        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        [Range(1800, int.MaxValue)]
        public int Year { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? Gearbox { get; set; }

        public string? ManufactureId { get; set; }

        // Add Id for the Car
        public string Id { get; set; }
    }
}
