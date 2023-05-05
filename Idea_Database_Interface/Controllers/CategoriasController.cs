using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("Emprendedores/Categorias/{action}")]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CategoriasController(IUnitOfWork uow) { _uow = uow; }

        public IActionResult Index()
        {
            CategoriasListViewModel vm = new()
            {
                Categorias = _uow.CategoriaRepository.GetAll().ToList()
            };
            return View(vm);
        }
        public IActionResult CreateCateg()
        {
            CategoriaCrudViewModel vm = new CategoriaCrudViewModel();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCateg(CategoriaCrudViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uow.CategoriaRepository.Create(new Categoría()
                {
                    Nombre = model.Nombre
                });
                await _uow.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> Overview(int id)
        {
            IEnumerable<Emprendedores> allEmps = _uow.EmprendedoresRepository.GetAll();
            List<EmprendedoresCategoría> emps = _uow.EmprendedoresCategoriaRepository.GetAll().Where(i => i.IdCategoría == id).ToList();
            List<Emprendedores> filtered = new List<Emprendedores>();
            foreach (var emp in emps)
            {
                Emprendedores temp = await _uow.EmprendedoresRepository.GetById(emp.IdEmprendedores);
                filtered.Add(temp);
            }
            CategoriaEmprendListViewModel vm = new CategoriaEmprendListViewModel()
            {
                Categoría = await _uow.CategoriaRepository.GetById(id),
                FilterText = string.Empty,
                FilterVal = 1,
                Emprendedores = filtered
            };
            return View(vm);
        }
    }
}
