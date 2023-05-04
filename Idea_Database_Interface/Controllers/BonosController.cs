using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class BonosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
