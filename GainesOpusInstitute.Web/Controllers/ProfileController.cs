using Microsoft.AspNetCore.Mvc;

namespace GainesOpusInstitute.Web.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
