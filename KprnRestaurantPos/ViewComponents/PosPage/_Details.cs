using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.ViewComponents.PosPage
{
    public class _Details : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
