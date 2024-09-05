using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos
{
    public class AddProductDto
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductOrder { get; set; }
        public bool ProductStatus { get; set; }
    }
}
