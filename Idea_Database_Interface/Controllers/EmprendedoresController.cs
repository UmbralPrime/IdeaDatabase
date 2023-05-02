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
                if (updateCat != null)
                {
                    if (model.Categorias == null)
                        model.Categorias = new List<Categoría>();
                    foreach (int item in model.SelectedCategorias)
                    {
                        model.Categorias.Add(await _uow.CategoriaRepository.GetById(item));
                    }
                    updateCat.Categorias = model.Categorias;
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

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
