using DataAccess.Layer.Abstract;
using DataAccess.Layer.Repository;
using Entity.Layer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Layer.EntityFramework
{
    public class EfSupplierDal : GenericRepository<Supplier>, ISupplierDal
    {
    }
}
