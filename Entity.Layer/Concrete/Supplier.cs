using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Supplier
    {
        [Key]
        public int? SupplierId { get; set; }
        public string? Company { get; set; } = "";
        public string? Phone { get; set; } = "";
        public string? Email { get; set; } = ""; 
        public string? Address { get; set; } = "";
    }
}
