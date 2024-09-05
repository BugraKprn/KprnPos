namespace KprnRestaurantPos.Models
{
    public class TableViewModel
    {
        public int? TableId { get; set; }
        public string? TableName { get; set; }
        public bool? IsOccupied { get; set; } // Masa dolu mu?
        public DateTime? OrderDate { get; set; } // Sipariş Tarihi
        public int? OrderCount { get; set; }  // Sipariş sayısı
        public decimal? OrderTotalPrice { get; set; }  // Siparişlerin Toplam Fiyatı

    }
}
