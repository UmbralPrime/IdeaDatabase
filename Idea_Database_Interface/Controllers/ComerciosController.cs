﻿using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using PagedList;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class ComerciosController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ComerciosController(IUnitOfWork uow) { _uow = uow; }
        public IActionResult Index(string searchString, string filterSelect, int? page)
        {
            List<string> options = new();
            options.Add("Nombre");
            options.Add("FUC");
            options.Add("Correo");
            SelectList filterOptions = new SelectList(options, filterSelect);
            IEnumerable<Comercios> comercios = _uow.ComerciosRepository.GetAll();

            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        comercios = comercios.Where(x => x.Nombre.ToLower().Contains(searchString));
                        break;
                    case "CIF":
                        comercios = comercios.Where(x => x.CódigoFUC.Contains(searchString));
                        break;
                    case "Email":
                        comercios = comercios.Where(x => x.Correo.ToLower().Contains(searchString));
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
            IPagedList testList = comercios.ToPagedList(1, pageSize);
            if (testList.PageCount < pageNumber)
                pageNumber = testList.PageCount;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            ComerciosListViewModel vm = new ComerciosListViewModel()
            {
                Comercios = comercios.ToPagedList(pageNumber, pageSize),
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
            ComercioDetailViewModel model = new ComercioDetailViewModel()
            {
                Comercio = await _uow.ComerciosRepository.GetById(id)
            };
            return View(model);
        }
        public async Task<IActionResult> UpdateComer(int id)
        {
            Comercios model = await _uow.ComerciosRepository.GetById(id);
            ComerciosCrudViewModel vm = new()
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
        public async Task<IActionResult> UpdateComer(int id, ComerciosCrudViewModel model)
        {
            model.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    Comercios comercio = new Comercios()
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

