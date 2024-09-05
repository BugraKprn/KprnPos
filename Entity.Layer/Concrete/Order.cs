using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderStatus { get; set; }

        //Table
        public int TableId { get; set; }
        public Table Table { get; set; }

        //List of OrderItem
        public List<OrderItem> OrderItems { get; set; }

        // Total Price
        public decimal TotalPrice { get; set; }
    }
}
