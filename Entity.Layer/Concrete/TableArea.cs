using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class TableArea
    {
        [Key]
        public int TableAreaId { get; set; }
        public string? AreaName { get; set; }
        public int? AreaOrder { get; set; }


        // İlişkiler
        public ICollection<Table>? Tables { get; set; }
    }
}
