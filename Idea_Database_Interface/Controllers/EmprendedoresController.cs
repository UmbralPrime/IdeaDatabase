﻿using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Data;
using PagedList;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmprendedoresController : Controller
    {
        private readonly IUnitOfWork _uow;
        public EmprendedoresController(IUnitOfWork uow) { _uow = uow; }
        public List<Categoría> Categorias;

        public IActionResult Index(string searchString, string filterSelect, int? page)
        {
            Categorias = _uow.CategoriaRepository.GetAll().ToList();
            IEnumerable<Emprendedores> emprendes = _uow.EmprendedoresRepository.GetAll().Include(p => p.Categorías).OrderByDescending(x => x.Fecha);
            List<string> options = new();
            options.Add("Nombre");
            options.Add("Teléfono");
            options.Add("Email");
            SelectList filterOptions = new SelectList(options);
            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        emprendes= emprendes.Where(x => x.Nombre.ToLower().Contains(searchString) || x.Apellidos.ToLower().Contains(searchString)).ToList();
                        break;
                    case "Teléfono":
                        emprendes = emprendes.Where(x => x.Teléfono.Contains(searchString)).ToList();
                        break;
                    case "Email":
                        emprendes = emprendes.Where(x => x.Email.ToLower().Contains(searchString)).ToList();
                        break;
                    default:
                        break;
                }
            }
            //This is to prevent the searchbox from being erased
            ViewBag.SearchString = searchString;
            int pageNumber = page ?? 1;
            int pageSize = 10;
            //this is to check if the pagenumber isnt higher than the pagecount
            IPagedList testList = emprendes.ToPagedList(1, pageSize);
            if (testList.PageCount < pageNumber)
                pageNumber = testList.PageCount;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            EmprendedoresListViewModel vm = new()
            {
                Emprendedores = emprendes.ToPagedList(pageNumber,pageSize),
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
        public IActionResult CreateEmprend()
        {
            EmprendedoresCrudViewModel vm = new()
            {
                //This code will fill the select box with all the categories.
                //It will show the name and the value that is connected is the id,
                //so it can be used to create the association link with the entrepeneur
                AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll().Where(x=>x.IsActive==true), "Id", "Nombre")
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
                model.AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll().Where(x=>x.IsActive==true), "Id", "Nombre");
                return View(model);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            Categorias = _uow.CategoriaRepository.GetAll().ToList();
            EmprendedoresDetailsViewModel vm = new EmprendedoresDetailsViewModel()
            {
                Emprendedores = await _uow.EmprendedoresRepository.GetById(id),
                CatYears = _uow.CatYearRepository.GetAll().ToList()
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
                AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll().Where(x => x.IsActive == true), "Id", "Nombre"),
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
                model.AllCategorias = new MultiSelectList(_uow.CategoriaRepository.GetAll().Where(x => x.IsActive == true), "Id", "Nombre");
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
