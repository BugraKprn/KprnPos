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
    public class SettingManager : ISettingService
    {
        ISettingDal _settingDal;

        public SettingManager(ISettingDal settingDal)
        {
            _settingDal = settingDal;
        }

        public void TAdd(Setting t)
        {
            _settingDal.Insert(t);
        }

        public void TDelete(Setting t)
        {
            _settingDal.Delete(t);
        }

        public Setting TGetById(int id)
        {
            return _settingDal.GetByID(id);
        }

        public List<Setting> TGetList()
        {
            return _settingDal.GetList();
        }

        public void TUpdate(Setting t)
        {
            _settingDal.Update(t);
        }
    }
}
