using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutor.Models
{
    [Table("VisitLogs")]
    public class VisitLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string IpAddress { get; set; }
    }
}
