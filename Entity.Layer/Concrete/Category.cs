using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Category
    {
        [Key]
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryIcon { get; set; }
        public string? CategoryMenuImage { get; set; }
        public int? CategoryOrder { get; set; } = 0;
        public bool? CategoryStatus { get; set; } = false;
        public ICollection<Product>? Products { get; set; }

    }
}
