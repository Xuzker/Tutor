using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Tutor.Models
{
    public class Application
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public long CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
    }
}
