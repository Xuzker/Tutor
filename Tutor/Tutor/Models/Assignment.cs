using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Models
{
    public class Assignment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public long CourseId { get; set; }
        public Course Course { get; set; }
    }
}
