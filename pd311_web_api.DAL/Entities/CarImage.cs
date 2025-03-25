using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class CarImage
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();  // Unique Id for each image

    [Required]
    public string FileName { get; set; }  // The filename, not the primary key

    [Required]
    public string FilePath { get; set; } // Path to the file on the server

    public string CarId { get; set; } // Car identifier
    [ForeignKey("CarId")]
    public Car Car { get; set; } // Relationship with Car
}
