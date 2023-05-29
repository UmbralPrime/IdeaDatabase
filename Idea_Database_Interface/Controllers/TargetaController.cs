using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using System.Data;

namespace Idea_Database_Interface.Controllers
{
    public class TargetaController : Controller
    {
        private readonly IUnitOfWork _uow;
        public TargetaController(IUnitOfWork uow) { _uow = uow; }
        public IActionResult Index(string searchString, string filterSelect, int? page)
        {
            List<string> options = new();
            options.Add("Nombre");
            options.Add("FUC");
            options.Add("Correo");
            SelectList filterOptions = new SelectList(options, filterSelect);
            IEnumerable<Targeta> targetas = _uow.TargetasRepository.GetAll();

            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        targetas = targetas.Where(x => x.Nombre.ToLower().Contains(searchString));
                        break;
                    case "CIF":
                        targetas = targetas.Where(x => x.CódigoFUC.Contains(searchString));
                        break;
                    case "Email":
                        targetas = targetas.Where(x => x.Correo.ToLower().Contains(searchString));
                        break;
                    default:
                        break;
                };
            }
            //This is to prevent the searchbox from being erased
            ViewBag.SearchString = searchString;
            int pageNumber = page ?? 1;
            int pageSize = 10;
            //this is to check if the pagenumber isnt higher than the pagecount
            IPagedList testList = targetas.ToPagedList(1, pageSize);
            if (testList.PageCount < pageNumber)
                pageNumber = testList.PageCount;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            TargetaListViewModel vm = new TargetaListViewModel()
            {
                Targetas = targetas.ToPagedList(pageNumber, pageSize),
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
        public IActionResult CreateTargeta()
        {
            TargetaCrudViewModel vm = new TargetaCrudViewModel();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTargeta(TargetaCrudViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uow.TargetasRepository.Create(new Targeta()
                {
                    Nombre = model.Nombre,
                    IAE = model.IAE,
                    CódigoFUC = model.CódigoFUC,
                    TeléfonoMóvil = model.TeléfonoMóvil,
                    TipoDeVia = model.TipoDeVia,
                    Dirección = model.Dirección,
                    TeléfonoFijo = model.TeléfonoFijo,
                    Correo = model.Correo
                });
                await _uow.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            TargetaDetailViewModel model = new TargetaDetailViewModel()
            {
                Targeta = await _uow.TargetasRepository.GetById(id)
            };
            return View(model);
        }
        public async Task<IActionResult> UpdateTargeta(int id)
        {
            Targeta model = await _uow.TargetasRepository.GetById(id);
            TargetaCrudViewModel vm = new()
            {
                Id = id,
                Nombre = model.Nombre,
                IAE = model.IAE,
                CódigoFUC = model.CódigoFUC,
                TeléfonoMóvil = model.TeléfonoMóvil,
                TipoDeVia = model.TipoDeVia,
                Dirección = model.Dirección,
                TeléfonoFijo = model.TeléfonoFijo,
                Correo = model.Correo
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTargeta(int id, TargetaCrudViewModel model)
        {
            model.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    Targeta targeta = new Targeta()
                    {
                        Id = id,
                        Nombre = model.Nombre,
                        IAE = model.IAE,
                        CódigoFUC = model.CódigoFUC,
                        TeléfonoMóvil = model.TeléfonoMóvil,
                        TipoDeVia = model.TipoDeVia,
                        Dirección = model.Dirección,
                        TeléfonoFijo = model.TeléfonoFijo,
                        Correo = model.Correo
                    };
                    _uow.TargetasRepository.Update(targeta);
                    await _uow.Save();
                }
                catch (DBConcurrencyException ex)
                {
                    if (await _uow.TargetasRepository.GetById(id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", new { id = id });
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteTargeta(int id)
        {
            TargetaDetailViewModel model = new TargetaDetailViewModel() { Targeta = await _uow.TargetasRepository.GetById(id) };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTargeta(int id, TargetaDetailViewModel model)
        {
            Targeta targeta = await _uow.TargetasRepository.GetById(id);
            if (targeta != null)
            {
                _uow.TargetasRepository.Delete(targeta);
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
