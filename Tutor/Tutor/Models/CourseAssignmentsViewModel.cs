using System.Collections.Generic;

namespace Tutor.Models
{
    public class CourseAssignmentsViewModel
    {
        public string CourseName { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
