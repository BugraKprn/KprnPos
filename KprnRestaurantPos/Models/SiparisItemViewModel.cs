namespace KprnRestaurantPos.Models
{
    public class SiparisItemViewModel
    {
        public int ProductId { get; set; } // Ürün Id
        public string? ProductName { get; set; } // Ürün adı
        public string? ProductImage { get; set; } // Ürün resmi
        public int Quantity { get; set; } // Adet
        public decimal? UnitPrice { get; set; } // Birim fiyat
        public int OrderId { get; set; }
    }

}
