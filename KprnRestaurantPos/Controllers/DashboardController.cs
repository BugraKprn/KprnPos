using DataAccess.Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    [Route("/Pos/Dashboard")]
    public class DashboardController : Controller
    {
        Context Context = new Context();
        public IActionResult Index()
        {
            var deger1 = Context.Products.Count().ToString();
            var deger2 = Context.Categories.Count().ToString();
            var deger3 = Context.Tables.Count().ToString();




            ViewBag.v1 = deger1;
            ViewBag.v2 = deger2;
            ViewBag.v3 = deger3;
            return View();
        }
    }
}
