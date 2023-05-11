using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using PagedList;
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
        public async Task<IActionResult> Overview(int id, string searchString, string filterSelect,int? page)
        {
            IEnumerable<Emprendedores> allEmps = _uow.EmprendedoresRepository.GetAll();
            List<EmprendedoresCategoría> emps = _uow.EmprendedoresCategoriaRepository.GetAll().Where(i => i.IdCategoría == id).ToList();
            List<Emprendedores> filtered = new List<Emprendedores>();
            //add all the emprendedores based on the category id
            foreach (var emp in emps)
            {
                Emprendedores temp = await _uow.EmprendedoresRepository.GetById(emp.IdEmprendedores);
                filtered.Add(temp);
            }
            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all 
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        filtered = filtered.Where(x => x.Nombre.ToLower().Contains(searchString) || x.Apellidos.ToLower().Contains(searchString)).ToList();
                        break;
                    case "Teléfono":
                        filtered = filtered.Where(x => x.Teléfono.Contains(searchString)).ToList();
                        break;
                    case "Email":
                        filtered = filtered.Where(x => x.Email.ToLower().Contains(searchString)).ToList();
                        break;
                    default:
                        break;
                }
            }
            List<string> options = new();
            options.Add("Nombre");
            options.Add("Teléfono");
            options.Add("Email");
            SelectList filterOptions = new SelectList(options);
            //This is to prevent the searchbox from being erased
            ViewBag.SearchString = searchString;
            int pageNumber = page ?? 1;
            int pageSize = 10;
            //this is to check if the pagenumber isnt higher than the pagecount
            IPagedList testList = filtered.ToPagedList(1, pageSize);
            if (testList.PageCount < pageNumber)
                pageNumber = testList.PageCount;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            CategoriaEmprendListViewModel vm = new CategoriaEmprendListViewModel()
            {
                Categoría = await _uow.CategoriaRepository.GetById(id),
                Emprendedores = filtered.ToPagedList(pageNumber,pageSize),
                FilterOptions = filterOptions,
                PageCount = pageNumber,
                SearchedFilter = filterSelect,
                SearchedString = searchString
            };
            //This resets the pagenumber for the next and previous page buttons
            //Without this you can only change the page once
            //And now it just works, i dont know how
            ModelState.Clear();
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
