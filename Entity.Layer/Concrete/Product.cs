using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Product
    {
        [Key]
        public int? ProductId { get; set; }
        public string? ProductName { get; set; } = "";
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; } = 0;
        public string? ProductImage { get; set; }
        public int? ProductQuantity { get; set; } = 1;
        public int? ProductOrder { get; set; } = 0;
        public bool? ProductStatus { get; set; } = false;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
