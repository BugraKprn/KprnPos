using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        //Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        //Product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
