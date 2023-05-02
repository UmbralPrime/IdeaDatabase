using Microsoft.AspNetCore.Mvc;

namespace Idea_Database_Interface.Controllers
{
    public class BonosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
