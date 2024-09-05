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
        public string? TableName { get; set; }
        public string? IsOccupied { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.Now;

        // Bölge ile ilişki
        public int TableAreaId { get; set; }
        public TableArea TableArea { get; set; }

        //Masa Konumları
        public double PosX { get; set; } // X koordinatı
        public double PosY { get; set; } // Y koordinatı


        public ICollection<Order>? Orders { get; set; }
    }
}
