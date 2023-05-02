using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmprendedoresController : Controller
    {
        private readonly IUnitOfWork _uow;
        public EmprendedoresController(IUnitOfWork uow) { _uow = uow; }

        public IActionResult Index()
        {
            EmprendedoresListViewModel vm = new()
            {
                Emprendedores = _uow.EmprendedoresRepository.GetAll().ToList(),
                FilterText = "",
                FilterVal = 1
            };
            return View(vm);
        }
        public IActionResult CreateEmprend()
        {
            return View();
        }
        public IActionResult Categorias()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
