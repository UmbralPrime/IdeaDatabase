using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class ComerciosController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ComerciosController(IUnitOfWork uow) { _uow = uow; }
        public IActionResult Index(string searchString, string filterSelect)
        {
            List<string> options = new();
            options.Add("Nombre");
            options.Add("CIF");
            options.Add("Email");
            SelectList filterOptions = new SelectList(options);
            ComerciosListViewModel vm = new ComerciosListViewModel()
            {
                Comercios = _uow.ComerciosRepository.GetAll(),
                FilterOptions = filterOptions
            };
            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        vm.Comercios = _uow.ComerciosRepository.GetAll().Where(x => x.NombreComercial.ToLower().Contains(searchString));
                        break;
                    case "CIF":
                        vm.Comercios = _uow.ComerciosRepository.GetAll().Where(x => x.CIF.Contains(searchString));
                        break;
                    case "Email":
                        vm.Comercios = _uow.ComerciosRepository.GetAll().Where(x => x.Email.ToLower().Contains(searchString));
                        break;
                    default:
                        break;
                };
            }
            return View(vm);
        }

        public IActionResult CreateComer()
        {
            ComerciosCrudViewModel vm = new ComerciosCrudViewModel();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComer(ComerciosCrudViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uow.ComerciosRepository.Create(new Comercios()
                {
                    NombreComercial = model.NombreComercial,
                    CIF = model.CIF,
                    IAE = model.IAE,
                    CódigoFUC = model.CódigoFUC,
                    Móvi = model.Móvi,
                    Calle = model.Calle,
                    Número = model.Número,
                    CódigoPostal = model.CódigoPostal,
                    Provincia = model.Provincia,
                    Municipio = model.Municipio,
                    Localidad = model.Localidad,
                    TeléfonoFijo = model.TeléfonoFijo,
                    Email = model.Email
                });
                await _uow.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            ComercioDetailViewModel model = new ComercioDetailViewModel()
            {
                Comercio = await _uow.ComerciosRepository.GetById(id)
            };
            return View(model);
        }
        public async Task<IActionResult> UpdateComer(int id)
        {
            Comercios temp = await _uow.ComerciosRepository.GetById(id);
            ComerciosCrudViewModel model = new()
            {
                Calle = temp.Calle,
                CIF = temp.CIF,
                CódigoFUC = temp.CódigoFUC,
                Provincia = temp.Provincia,
                CódigoPostal = temp.CódigoPostal,
                NombreComercial = temp.NombreComercial,
                Email = temp.Email,
                IAE = temp.IAE,
                Id = id,
                Localidad = temp.Localidad,
                Municipio = temp.Municipio,
                Móvi = temp.Móvi,
                Número = temp.Número,
                TeléfonoFijo = temp.TeléfonoFijo
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateComer(int id, ComerciosCrudViewModel model)
        {
            model.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    Comercios comercio = new Comercios()
                    {
                        Calle = model.Calle,
                        CIF = model.CIF,
                        CódigoFUC = model.CódigoFUC,
                        Provincia = model.Provincia,
                        CódigoPostal = model.CódigoPostal,
                        NombreComercial = model.NombreComercial,
                        Email = model.Email,
                        IAE = model.IAE,
                        Id = id,
                        Localidad = model.Localidad,
                        Municipio = model.Municipio,
                        Móvi = model.Móvi,
                        Número = model.Número,
                        TeléfonoFijo = model.TeléfonoFijo
                    };
                    _uow.ComerciosRepository.Update(comercio);
                    await _uow.Save();
                }
                catch (DBConcurrencyException ex)
                {
                    if (await _uow.ComerciosRepository.GetById(id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", new { id = id });
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteComer(int id)
        {
            ComercioDetailViewModel model = new ComercioDetailViewModel() { Comercio = await _uow.ComerciosRepository.GetById(id) };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComer(int id, ComercioDetailViewModel model)
        {
            Comercios comer = await _uow.ComerciosRepository.GetById(id);
            if (comer != null)
            {
                _uow.ComerciosRepository.Delete(comer);
                await _uow.Save();
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

