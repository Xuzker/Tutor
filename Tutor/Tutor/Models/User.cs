using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tutor.Models;


namespace Tutor.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
