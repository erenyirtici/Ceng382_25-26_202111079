using System.ComponentModel.DataAnnotations;

namespace Week5Lab.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        [StringLength(100, ErrorMessage = "Class Name cannot exceed 100 characters.")]
        public string ClassName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Student Count is required.")]
        [Range(1, 1000, ErrorMessage = "Student count must be between 1 and 1000.")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
