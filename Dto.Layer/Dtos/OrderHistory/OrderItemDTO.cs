using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos.OrderHistory
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
