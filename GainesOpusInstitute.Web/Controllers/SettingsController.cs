using Microsoft.AspNetCore.Mvc;

namespace GainesOpusInstitute.Web.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
