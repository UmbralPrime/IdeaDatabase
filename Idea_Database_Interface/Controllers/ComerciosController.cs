using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class ComerciosController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ComerciosController(IUnitOfWork uow) { _uow = uow; }
        public IActionResult Index()
        {
            ComerciosListViewModel vm = new ComerciosListViewModel()
            {
                Comercios = _uow.ComerciosRepository.GetAll()
            };
            return View(vm);
        }
        public IActionResult Filter(CompaniesListViewModel model)
        {
            //This is the same filter that is used in the companies page
            ComerciosListViewModel vm = new ComerciosListViewModel()
            {
                Comercios = _uow.ComerciosRepository.GetAll(),
                FilterText = string.Empty,
                FilterVal = 1
            };
            if (model.FilterText != null)
            {

                model.FilterText = model.FilterText.ToLower();
                switch (model.FilterVal)
                {
                    case 1:
                        vm.Comercios = _uow.ComerciosRepository.GetAll().Where(x => x.NombreComercial.ToLower().Contains(model.FilterText));
                        return View("Index", vm);
                    case 2:
                        vm.Comercios = _uow.ComerciosRepository.GetAll().Where(x => x.CIF.Contains(model.FilterText));
                        return View("Index", vm);
                    case 3:
                        vm.Comercios = _uow.ComerciosRepository.GetAll().Where(x => x.Email.ToLower().Contains(model.FilterText));
                        return View("Index", vm);
                    default:
                        return View("Index", vm);
                }
            }
            return View("Index", vm);
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
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteComer(int id)
        {
            ComercioDetailViewModel model = new ComercioDetailViewModel() {Comercio = await _uow.ComerciosRepository.GetById(id) };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComer(int id,  ComercioDetailViewModel model)
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

