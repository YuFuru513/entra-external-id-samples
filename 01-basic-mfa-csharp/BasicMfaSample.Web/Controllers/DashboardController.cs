using Microsoft.AspNetCore.Mvc;

namespace BasicMfaSample.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
