using pd311_web_api.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Car : BaseEntity<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();

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
    public string Color { get; set; }

    [MaxLength(50)]
    public string Gearbox { get; set; }

    public string ManufactureId { get; set; }
    [ForeignKey("ManufactureId")]
    public Manufacture Manufacture { get; set; }

    // Navigation property for related CarImages
    public ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();
}
