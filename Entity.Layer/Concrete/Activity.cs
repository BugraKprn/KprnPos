using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Activity
    {
        [Key]
        public int? ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ActivityType { get; set; }
        public bool? ActivityStatus { get; set; }
    }
}
