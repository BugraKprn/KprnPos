using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos.TableAndArea
{
    public class TableBulkAddDto
    {
        public string TableName { get; set; }
        public int TableCount { get; set; }
        public int TableAreaId { get; set; }
        public string TableShape { get; set; }
        public string? IsOccupied { get; set; }

        public double PosX { get; set; }
        public double PosY { get; set; }
    }
}
