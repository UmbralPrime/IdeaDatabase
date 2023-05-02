using Microsoft.AspNetCore.Mvc;

namespace Idea_Database_Interface.Controllers
{
    public class ComerciosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
