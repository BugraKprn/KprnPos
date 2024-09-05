using Business.Layer.Abstract;
using DataAccess.Layer.Abstract;
using DataAccess.Layer.EntityFramework;
using Entity.Layer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Layer.Concrete
{
    public class SupplierManager : ISupplierService
    {
        ISupplierDal _supplierDal;

        public SupplierManager(ISupplierDal supplierDal)
        {
            _supplierDal = supplierDal;
        }
        public void TAdd(Supplier t)
        {
            _supplierDal.Insert(t);
        }

        public void TDelete(Supplier t)
        {
            _supplierDal.Delete(t);
        }

        public Supplier TGetById(int id)
        {
            return _supplierDal.GetByID(id);
        }

        public List<Supplier> TGetList()
        {
            return _supplierDal.GetList();
        }

        public void TUpdate(Supplier t)
        {
            _supplierDal.Update(t);
        }
    }
}
