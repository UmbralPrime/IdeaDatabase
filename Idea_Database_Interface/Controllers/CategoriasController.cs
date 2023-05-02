using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Mvc;

namespace Idea_Database_Interface.Controllers
{
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
            if(ModelState.IsValid)
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
    }
}
