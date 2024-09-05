using DataAccess.Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        Context context = new Context();

        [Route("/Pos/Calendar")]
        public IActionResult Index()
        {
            var etkinlikler = context.Activities.Select(e => new {
                ActivityId = e.ActivityId,
                ActivityName = e.ActivityName,
                StartDate = e.StartDate.HasValue ? e.StartDate.Value.ToString("yyyy-MM-dd") : null,
                EndDate = e.EndDate.HasValue ? e.EndDate.Value.ToString("yyyy-MM-dd") : null,
                ActivityType = e.ActivityType,
                ActivityStatus = e.ActivityStatus,
                Color = GetColorByActivityType(e.ActivityType)
            }).ToList();

            return View(etkinlikler);
        }

        private static string GetColorByActivityType(string activityType)
        {
            if (activityType == "Doğum Günü")
            {
                return "#C40C0C"; // Kırmızı renk
            }
            else if (activityType == "Toplantı")
            {
                return "#1A4D2E"; // Yeşil renk
            }
            else if (activityType == "Kına & Nişan")
            {
                return "#8576FF"; // Yeşil renk
            }
            else
            {
                return "#0C0C0C"; // Varsayılan renk siyah
            }
        }
    }


}
