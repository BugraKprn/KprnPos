using Entity.Layer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos.TableAndArea
{
    public class TableAndAreaViewModel
    {
        public List<TableArea> TableAreas { get; set; }
        public List<Table> Tables { get; set; }
    }
}
