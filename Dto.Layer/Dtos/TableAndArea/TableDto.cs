using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos.TableAndArea
{
    public class TableDto
    {
        public int? TableId { get; set; }
        public string? TableName { get; set; }
        public bool? IsOccupied { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
