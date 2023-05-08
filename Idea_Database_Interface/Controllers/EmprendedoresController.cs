using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
            IEnumerable<Emprendedores> emprendes = _uow.EmprendedoresRepository.GetAll().Include(p => p.Categorías).OrderByDescending(x => x.Fecha);
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
            //This is the same filter that is used in the companies page
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
                //This code will fill the select box with all the categories.
                //It will show the name and the value that is connected is the id,
                //so it can be used to create the association link with the entrepeneur
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
                    Observaciones = model.Observaciones,
                    PlanViabilidad = model.PlanViabilidad,
                    Terminado = model.Terminado
                });
                await _uow.Save();
                //The following code will add the categories to the freshly created entrepeneur.
                //It will also check to see if the categories are already linked.
                //Wich is redundant, so i will comment it
                Emprendedores updateCat = _uow.EmprendedoresRepository.GetAll()
                    .Where(p => p.Fecha == model.Fecha + model.FechaHora && p.Nombre == model.Nombre).FirstOrDefault();
                //IQueryable<EmprendedoresCategoría> check = _uow.EmprendedoresCategoriaRepository.GetAll();
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
                        //if (check.Where(x => x.IdCategoría == tempAdd.IdCategoría && x.IdEmprendedores == tempAdd.IdEmprendedores) == null)
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
            Categorias = _uow.CategoriaRepository.GetAll().ToList();
            EmprendedoresDetailsViewModel vm = new EmprendedoresDetailsViewModel()
            {
                Emprendedores = await _uow.EmprendedoresRepository.GetById(id)
            };
            vm.Emprendedores.Categorías = _uow.EmprendedoresCategoriaRepository.GetAll().Where(p => p.IdEmprendedores == id).Include(i => i.Categoría).ToList();
            return View(vm);
        }
        public async Task<IActionResult> UpdateEmprend(int id)
        {
            Emprendedores temp = await _uow.EmprendedoresRepository.GetById(id);
            //this code is to show the selected categories, but it doesnt work yet
            int[] selection = new int[] { };
            if (temp.Categorías != null)
            {
                foreach (var cat in temp.Categorías)
                {
                    selection.Append(cat.Id);
                }
            }

            EmprendedoresCrudViewModel vm = new EmprendedoresCrudViewModel()
            {
                Id = id,
                Apellidos = temp.Apellidos,
                Nombre = temp.Nombre,
                Email = temp.Email,
                Fecha = temp.Fecha,
                Incidencias = temp.Incidencias,
                MotivoDeLaConsulto = temp.Incidencias,
                Observaciones = temp.Observaciones,
                PlanViabilidad = temp.PlanViabilidad,
                Teléfono = temp.Teléfono,
                Terminado = temp.Terminado,
                AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll(), "Id", "Nombre"),
                SelectedCategorias = selection
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmprend(EmprendedoresCrudViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uow.EmprendedoresRepository.Update(new Emprendedores()
                {
                    Id = (int)model.Id,
                    Fecha = model.Fecha + model.FechaHora,
                    Nombre = model.Nombre,
                    Apellidos = model.Apellidos,
                    Teléfono = model.Teléfono,
                    Email = model.Email,
                    MotivoDeLaConsulto = model.MotivoDeLaConsulto,
                    Incidencias = model.Incidencias,
                    Observaciones = model.Observaciones,
                    PlanViabilidad = model.PlanViabilidad,
                    Terminado = model.Terminado
                });
                await _uow.Save();
                Emprendedores updateCat = await _uow.EmprendedoresRepository.GetById((int)model.Id);
                IQueryable<EmprendedoresCategoría> check = _uow.EmprendedoresCategoriaRepository.GetAll();
                //checks to see if the association is already in the database, otherwise it will add it
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
                        IQueryable<EmprendedoresCategoría> chenck = check.Where(x => x.IdCategoría == tempAdd.IdCategoría && x.IdEmprendedores == tempAdd.IdEmprendedores);
                        if (check.Where(x => x.IdCategoría == tempAdd.IdCategoría && x.IdEmprendedores == tempAdd.IdEmprendedores).Count() == 0)
                            model.Categorias.Add(tempAdd);
                    }
                    updateCat.Categorías = model.Categorias;
                    _uow.EmprendedoresRepository.Update(updateCat);
                    await _uow.Save();

                }


                return RedirectToAction("Details", new { id = model.Id });
            }
            else
            {
                model.AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll(), "Id", "Nombre");
                return View(model);
            }
        }
        public async Task<IActionResult> DeleteEmprend(int id)
        {
            Emprendedores temp = await _uow.EmprendedoresRepository.GetById(id);
            EmprendedoresCrudViewModel vm = new EmprendedoresCrudViewModel()
            {
                Id = id,
                Nombre = temp.Nombre,
                Apellidos = temp.Apellidos
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmprend(int id, EmprendedoresCrudViewModel vm)
        {
            Emprendedores empr = await _uow.EmprendedoresRepository.GetById(id);
            if (empr != null)
            {
                _uow.EmprendedoresRepository.Delete(empr);
                await _uow.Save();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteCategor(int catId, int id)
        {
            EmprendedoresCategoría toDelete = await _uow.EmprendedoresCategoriaRepository.GetById(catId);
            _uow.EmprendedoresCategoriaRepository.Delete(toDelete);
            await _uow.Save();
            Categorias = _uow.CategoriaRepository.GetAll().ToList();
            EmprendedoresDetailsViewModel vm = new EmprendedoresDetailsViewModel()
            {
                Emprendedores = await _uow.EmprendedoresRepository.GetById(id)
            };
            vm.Emprendedores.Categorías = _uow.EmprendedoresCategoriaRepository.GetAll().Where(p => p.IdEmprendedores == id).Include(i => i.Categoría).ToList();
            return RedirectToAction("Details", new { id = id });
        }
    }
}
