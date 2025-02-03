using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Tutor.Models;


namespace Tutor.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
