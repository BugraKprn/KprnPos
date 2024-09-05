using Business.Layer.Concrete;
using DataAccess.Layer.EntityFramework;
using Entity.Layer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.ViewComponents.PosPage
{
    public class _Cart : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
