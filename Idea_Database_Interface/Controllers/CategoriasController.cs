using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                Categorias = _uow.CategoriaRepository.GetAll().ToList(),
                EmprendedoresCategorias = _uow.EmprendedoresCategoriaRepository.GetAll().ToList(),
                CatYears = _uow.CatYearRepository.GetAll().ToList()
            };
            return View(vm);
        }
        public IActionResult CreateCateg()
        {
            CategoriaCrudViewModel vm = new CategoriaCrudViewModel()
            {
                Years = new SelectList(_uow.CatYearRepository.GetAll(), "Id", "Nombre")
            };
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
                    Nombre = model.Nombre,
                    IdYear = model.Year,
                    IsActive = model.IsActive
                });
                await _uow.Save();
                return RedirectToAction("Index");
            }
            else
            {
                model.Years = new SelectList(_uow.CatYearRepository.GetAll(), "Id", "Nombre");
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
        public IActionResult CreateYear()
        {
            CatYearCreateViewModel vm = new CatYearCreateViewModel();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateYear(CatYearCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uow.CatYearRepository.Create(new CatYear()
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
        public IActionResult Activar()
        {
            CategoriasListViewModel vm = new()
            {
                Categorias = _uow.CategoriaRepository.GetAll().ToList(),
                EmprendedoresCategorias = _uow.EmprendedoresCategoriaRepository.GetAll().ToList(),
                CatYears = _uow.CatYearRepository.GetAll().ToList()
            };
            return View(vm);
        }
        public async Task<IActionResult> ActivateCat(int id)
        {
            Categoría categoría = await _uow.CategoriaRepository.GetById(id);
            categoría.IsActive = !categoría.IsActive;
            _uow.CategoriaRepository.Update(categoría);
            await _uow.Save();
            return RedirectToAction("Activar");
        }
        public async Task<IActionResult> UpdateCateg(int id)
        {
            Categoría cat = await _uow.CategoriaRepository.GetById(id);
            CategoriaCrudViewModel vm = new CategoriaCrudViewModel()
            {
                Nombre = cat.Nombre,
                Id = id,
                IsActive = cat.IsActive,
                Year = cat.IdYear,
                Years = new SelectList(_uow.CatYearRepository.GetAll(), "Id", "Nombre")
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCateg(int id, CategoriaCrudViewModel model)
        {
            model.Id = id;
            if (ModelState.IsValid)
            {
                _uow.CategoriaRepository.Update(new Categoría()
                {
                    Nombre = model.Nombre,
                    IdYear = model.Year,
                    IsActive = model.IsActive,
                    Id = (int)model.Id
                });
                await _uow.Save();
                return RedirectToAction("Activar");
            }
            else
            {
                model.Years = new SelectList(_uow.CatYearRepository.GetAll(), "Id", "Nombre");
                return View(model);
            }
        }
    }
}
