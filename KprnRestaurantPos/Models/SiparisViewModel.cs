using Entity.Layer.Concrete;
using System.ComponentModel.DataAnnotations;

namespace KprnRestaurantPos.Models
{
    public class SiparisViewModel
    {
        public int TableId { get; set; }
        public int OrderNumber { get; set; }
        public bool OrderStatus { get; set; } = true; // Sipariş Durumu
        public decimal TotalPrice { get; set; } // Siparişe ait Toplam Tutar
        public DateTime OrderDate { get; set; } // Sipariş tarihi
        public List<SiparisItemViewModel> OrderItems { get; set; } // Sipariş kalemleri
    }

}
