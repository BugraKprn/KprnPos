using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.Controllers
{
    public class LayoutController : Controller
    {
        public PartialViewResult PartialSidebar()
        {
            return PartialView();
        }

        public PartialViewResult PartialFooter()
        {
            return PartialView();
        }

        public PartialViewResult PartialHead()
        {
            return PartialView();
        }

        public PartialViewResult PartialNavbar()
        {
            return PartialView();
        }

        public PartialViewResult PartialScript()
        {
            return PartialView();
        }
    }
}
