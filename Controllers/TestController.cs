using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
