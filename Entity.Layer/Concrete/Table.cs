using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Table
    {
        [Key]
        public int? TableId { get; set; }
        public string? TableName { get; set; } = "";
        public bool? IsOccupied { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.Now;


        public ICollection<Order>? Orders { get; set; }
    }
}
