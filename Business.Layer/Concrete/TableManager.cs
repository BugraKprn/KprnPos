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
    public class TableManager : ITableService
    {
        ITableDal _tableDal;

        public TableManager(ITableDal tableDal)
        {
            _tableDal = tableDal;
        }
        public void TAdd(Table t)
        {
            _tableDal.Insert(t);
        }

        public void TDelete(Table t)
        {
            _tableDal.Delete(t);
        }

        public Table TGetById(int id)
        {
            return _tableDal.GetByID(id);
        }

        public List<Table> TGetList()
        {
            return _tableDal.GetList();
        }

        public void TUpdate(Table t)
        {
            _tableDal.Update(t);
        }
    }
}
