using System.ComponentModel.DataAnnotations;

namespace Week5Lab.Models
{
    public class Class
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string ClassName { get; set; }  // Formdaki alanla eşleşsin

    [Required]
    public int StudentCount { get; set; }  // Aynı şekilde

    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true; // default true olabilir
}

}