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
    public class EmprendedoresController : Controller
    {
        private readonly IUnitOfWork _uow;
        public EmprendedoresController(IUnitOfWork uow) { _uow = uow; }
        public List<Categoría> Categorias;

        public IActionResult Index()
        {
            Categorias = _uow.CategoriaRepository.GetAll().ToList();
            IEnumerable<Emprendedores> emprendes = _uow.EmprendedoresRepository.GetAll().Include(p => p.Categorías);
            EmprendedoresListViewModel vm = new()
            {
                Emprendedores = emprendes,
                FilterText = "",
                FilterVal = 1
            };
            return View(vm);

        }
        public IActionResult Filter(CompaniesListViewModel model)
        {
            EmprendedoresListViewModel vm = new EmprendedoresListViewModel()
            {
                Emprendedores = _uow.EmprendedoresRepository.GetAll().ToList(),
                FilterText = string.Empty,
                FilterVal = 1
            };
            if (model.FilterText != null)
            {

                model.FilterText = model.FilterText.ToLower();
                switch (model.FilterVal)
                {
                    case 1:
                        vm.Emprendedores = _uow.EmprendedoresRepository.GetAll().ToList().Where(x => x.Nombre.ToLower().Contains(model.FilterText));
                        return View("Index", vm);
                    case 2:
                        vm.Emprendedores = _uow.EmprendedoresRepository.GetAll().ToList().Where(x => x.Teléfono.Contains(model.FilterText));
                        return View("Index", vm);
                    case 3:
                        vm.Emprendedores = _uow.EmprendedoresRepository.GetAll().ToList().Where(x => x.Email.ToLower().Contains(model.FilterText));
                        return View("Index", vm);
                    default:
                        return View("Index", vm);
                }
            }
            return View("Index", vm);
        }
        public IActionResult CreateEmprend()
        {
            EmprendedoresCrudViewModel vm = new()
            {
                AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll(), "Id", "Nombre")
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmprend(EmprendedoresCrudViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uow.EmprendedoresRepository.Create(new Emprendedores()
                {
                    Fecha = model.Fecha + model.FechaHora,
                    Nombre = model.Nombre,
                    Apellidos = model.Apellidos,
                    Teléfono = model.Teléfono,
                    Email = model.Email,
                    MotivoDeLaConsulto = model.MotivoDeLaConsulto,
                    Incidencias = model.Incidencias,
                    PlanViabilidad = model.PlanViabilidad
                });
                await _uow.Save();
                Emprendedores updateCat = _uow.EmprendedoresRepository.GetAll().Where(p => p.Fecha == model.Fecha + model.FechaHora).FirstOrDefault();
                if (updateCat != null && model.SelectedCategorias != null)
                {
                    if (model.Categorias == null)
                        model.Categorias = new List<EmprendedoresCategoría>();
                    foreach (int item in model.SelectedCategorias)
                    {
                        EmprendedoresCategoría tempAdd = new()
                        {
                            Emprendedores = updateCat,
                            IdEmprendedores = updateCat.Id,
                            IdCategoría = item,
                            Categoría = await _uow.CategoriaRepository.GetById(item),
                        };
                        model.Categorias.Add(tempAdd);
                    }
                    updateCat.Categorías = model.Categorias;
                    _uow.EmprendedoresRepository.Update(updateCat);
                    await _uow.Save();
                }


                return RedirectToAction("Index");
            }
            else
            {
                model.AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll(), "Id", "Nombre");
                return View(model);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            EmprendedoresDetailsViewModel vm = new EmprendedoresDetailsViewModel()
            {
                Emprendedores = await _uow.EmprendedoresRepository.GetById(id)
            };
            return View();
        }
    }
}
