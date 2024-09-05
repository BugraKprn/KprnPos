using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos.TableAndArea
{
    public class TablePositionUpdateDto
    {
        public int TableId { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
    }
}
