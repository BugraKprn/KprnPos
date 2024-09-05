using Entity.Layer.Concrete;
using System.ComponentModel.DataAnnotations.Schema;

namespace KprnRestaurantPos.Models
{
    public class AddProductImage
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; } = 0;
        public IFormFile? ProductImage { get; set; }
        public int? ProductQuantity { get; set; }
        public int? ProductOrder { get; set; }
        public bool? ProductStatus { get; set; }
        public int? CategoryId { get; set; }
    }
}
