using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Models
{
    public class VisitLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string IpAddress { get; set; }
    }
}
