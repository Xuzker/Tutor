using System.ComponentModel.DataAnnotations;
namespace Tutor.Models
{
    public class Course
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int DurationWeeks { get; set; }

        [Required]
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
