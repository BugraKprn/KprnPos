using DataAccess.Layer.Abstract;
using DataAccess.Layer.Concrete;
using DataAccess.Layer.Repository;
using Entity.Layer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Layer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        public List<Product> GetListWithCategory()
        {
            using(var context = new Context())
            {
                return context.Products.Include(x => x.Category).ToList();
            }
        }
    }
}
