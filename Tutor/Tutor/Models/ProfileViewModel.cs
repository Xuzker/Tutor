using System.Collections.Generic;

namespace Tutor.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<Course> Courses { get; set; }
    }
}
