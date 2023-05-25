using Idea_Database_Interface.Data;
using Idea_Database_Interface.Data.Repository;
using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PagedList;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        //This is to fill in the index page
        public IActionResult Index(string searchString, string filterSelect, int? page)
        {
            List<string> options = new();
            options.Add("Nombre");
            options.Add("Teléfono");
            options.Add("Email");
            options.Add("CIF");
            SelectList filterOptions = new SelectList(options);
            IEnumerable<Empresa> empresas = _uow.EmpresaRepository.GetAll().ToList().OrderBy(x => x.Nombre);

            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        empresas = empresas.Where(x => x.Nombre.ToLower().Contains(searchString)).ToList();
                        break;
                    case "Teléfono":
                        empresas = empresas.Where(x => x.Teléfono.Contains(searchString)).ToList();
                        break;
                    case "Email":
                        empresas = empresas.Where(x => x.Email.ToLower().Contains(searchString)).ToList();
                        break;
                    case "CIF":
                        empresas = empresas.Where(x => x.CIF.ToLower().Contains(searchString)).ToList();
                        break;
                    default:
                        break;
                }
            }
            //This is to prevent the searchbox from being erased
            ViewBag.SearchString = searchString;
            //Here you can change the amount of items on a page
            int pageSize = 10;
            int pageNumber = page ?? 1;
            //this is to check if the pagenumber isnt higher than the pagecount
            IPagedList testList = empresas.ToPagedList(1, pageSize);
            if (testList.PageCount < pageNumber)
                pageNumber = testList.PageCount;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            CompaniesListViewModel model = new CompaniesListViewModel()
            {
                Empresas = empresas.ToPagedList(pageNumber, pageSize),
                FilterOptions = filterOptions,
                PageCount = pageNumber,
                SearchedFilter = filterSelect,
                SearchedString = searchString
            };
            //This resets the pagenumber for the next and previous page buttons
            //Without this you can only change the page once
            //And now it just works, i dont know how
            ModelState.Clear();
            return View(model);
        }
        //This goes to the details page of a clicked company
        public async Task<IActionResult> DetailsAsync(int id)
        {
            EmpresaDetailsView model = new EmpresaDetailsView()
            {
                Empresa = await _uow.EmpresaRepository.GetById(id)
            };
            return View(model);
        }
        //The create view to create a company
        public IActionResult CreateEmpr()
        {
            CreateEmpresaView model = new CreateEmpresaView() { };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Tries to create the filled in company, if it doesnt work it goes back to the same page to try again.
        public async Task<IActionResult> CreateEmpr(CreateEmpresaView model)
        {
            if (ModelState.IsValid)
            {
                _uow.EmpresaRepository.Create(new Empresa()
                {
                    Nombre = model.Nombre,
                    CIF = model.CIF,
                    FechaDeAlta = model.FechaDeAlta,
                    Tamaño = model.Tamaño,
                    CNAE = model.CNAE,
                    DetalleCNAE = model.DetalleCNAE,
                    Calle = model.Calle,
                    Número = model.Número,
                    CódigoPostal = model.CódigoPostal,
                    Provincia = model.Provincia,
                    Municipio = model.Municipio,
                    Localidad = model.Localidad,
                    Teléfono = model.Teléfono,
                    Teléfono2 = model.Teléfono2,
                    Web = model.Web,
                    Email = model.Email,
                    Contacto = model.Contacto
                });
                await _uow.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }
        //Update view of the specified company
        public async Task<IActionResult> UpdateEmpr(int id)
        {
            Empresa empresa = await _uow.EmpresaRepository.GetById(id);
            UpdateEmpresaView vm = new UpdateEmpresaView()
            {
                EmprId = empresa.EmprId,
                Nombre = empresa.Nombre,
                CIF = empresa.CIF,
                FechaDeAlta = empresa.FechaDeAlta,
                Tamaño = empresa.Tamaño,
                CNAE = empresa.CNAE,
                DetalleCNAE = empresa.DetalleCNAE,
                Calle = empresa.Calle,
                Número = empresa.Número,
                CódigoPostal = empresa.CódigoPostal,
                Provincia = empresa.Provincia,
                Municipio = empresa.Municipio,
                Localidad = empresa.Localidad,
                Teléfono = empresa.Teléfono,
                Teléfono2 = empresa.Teléfono2,
                Web = empresa.Web,
                Email = empresa.Email,
                Contacto = empresa.Contacto
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the values of the to update company
        public async Task<IActionResult> UpdateEmpr(UpdateEmpresaView update, int id)
        {
            update.EmprId = id;
            if (ModelState.IsValid)
            {
                try
                {
                    Empresa empresa = new Empresa()
                    {
                        EmprId = update.EmprId,
                        Nombre = update.Nombre,
                        CIF = update.CIF,
                        FechaDeAlta = update.FechaDeAlta,
                        Tamaño = update.Tamaño,
                        CNAE = update.CNAE,
                        DetalleCNAE = update.DetalleCNAE,
                        Calle = update.Calle,
                        Número = update.Número,
                        CódigoPostal = update.CódigoPostal,
                        Provincia = update.Provincia,
                        Municipio = update.Municipio,
                        Localidad = update.Localidad,
                        Teléfono = update.Teléfono,
                        Teléfono2 = update.Teléfono2,
                        Web = update.Web,
                        Email = update.Email,
                        Contacto = update.Contacto
                    };
                    _uow.EmpresaRepository.Update(empresa);
                    await _uow.Save();
                    return RedirectToAction("Details", new { id = empresa.EmprId });
                }
                catch (DBConcurrencyException ex)
                {
                    if (await _uow.EmpresaRepository.GetById(update.EmprId) == null)
                        return NotFound();
                    else
                        throw;
                }                
            }
            return View(update);
        }
        //delete view of the to delete company
        public async Task<IActionResult> DeleteEmpr(int id)
        {
            Empresa emp = await _uow.EmpresaRepository.GetById(id);
            if (emp != null)
            {
                DeleteViewModel vm = new DeleteViewModel()
                {
                    EmprId = emp.EmprId,
                    Empresa = emp
                };
                return View(vm);
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Deletes the selected company
        public async Task<IActionResult> DeleteEmpr(int id, Empresa emp)
        {
            Empresa empr = await _uow.EmpresaRepository.GetById(id);
            if (empr != null)
            {
                _uow.EmpresaRepository.Delete(empr);
                await _uow.Save();
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //overview of all the messages from the specified company
        public async Task<IActionResult> CorrespondOverview(int id)
        {
            Empresa empr = await _uow.EmpresaRepository.GetById(id);
            if (empr == null)
                return NotFound();
            CorrespondListViewModel model = new CorrespondListViewModel()
            {
                Empresa = empr,
                EmprId = id,
                Correspondencias = _uow.CorrespondenciaRepository.GetAll().Where(p => p.Empresa.EmprId == id).ToList()
            };
            return View(model);
        }
        //create message view for specified company
        public async Task<IActionResult> CreateCorrespond(int id)
        {
            Empresa empr = await _uow.EmpresaRepository.GetById(id);
            if (empr == null)
                return NotFound();
            CreateCorrespondViewModel model = new CreateCorrespondViewModel()
            {
                EmprId = id
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //actually creates the message
        public async Task<IActionResult> CreateCorrespond(int id, CreateCorrespondViewModel model)
        {
            Empresa empr = await _uow.EmpresaRepository.GetById(id);
            model.EmprId = id;
            if (empr == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                _uow.CorrespondenciaRepository.Create(new Correspondencia()
                {
                    Cuando = model.Cuando,
                    Remitente = model.Remitente,
                    Empresa = empr,
                    EmprId = id,
                    Mensaje = model.Mensaje,
                });
                await _uow.Save();
                return RedirectToAction("CorrespondOverview", new { id = id });
            }
            else
                return View(model);
        }
        //update view of the clicked message
        public async Task<IActionResult> UpdateCorrespond(int id)
        {
            Correspondencia cor = await _uow.CorrespondenciaRepository.GetById(id);
            if (cor == null)
                return NotFound();
            UpdateCorrespondViewModel vm = new UpdateCorrespondViewModel()
            {
                CorrespondId = id,
                Cuando = cor.Cuando,
                Remitente = cor.Remitente,
                Mensaje = cor.Mensaje,
                EmprId = cor.EmprId
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //updates the message with the updated values
        public async Task<IActionResult> UpdateCorrespond(int id, UpdateCorrespondViewModel viewModel)
        {
            Correspondencia cor = await _uow.CorrespondenciaRepository.GetById(id);
            if (cor == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    Correspondencia corre = new Correspondencia()
                    {
                        CorrespondId = id,
                        EmprId = cor.EmprId,
                        Cuando = viewModel.Cuando,
                        Remitente = viewModel.Remitente,
                        Mensaje = viewModel.Mensaje
                    };
                    _uow.CorrespondenciaRepository.Update(corre);
                    await _uow.Save();
                }
                catch (DBConcurrencyException ex)
                {
                    if (_uow.CorrespondenciaRepository.GetById(viewModel.CorrespondId) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("CorrespondOverview", new { id = cor.EmprId });
            }
            return View(viewModel);
        }
        //shows the details of the message that is clicked
        public async Task<IActionResult> DetailsCorrespond(int id)
        {
            Correspondencia cor = await _uow.CorrespondenciaRepository.GetById(id);
            if (cor == null) return NotFound();
            CorrespondDetailsViewModel viewModel = new CorrespondDetailsViewModel()
            {
                CorrespondId = id,
                EmprId = cor.EmprId,
                Cuando = cor.Cuando,
                Remitente = cor.Remitente,
                Mensaje = cor.Mensaje,
                Empresa = await _uow.EmpresaRepository.GetById(cor.EmprId)
            };
            return View(viewModel);
        }
        //shows the delete view of the message
        public async Task<IActionResult> DeleteCorrespond(int id)
        {
            Correspondencia cor = await _uow.CorrespondenciaRepository.GetById(id);
            if (cor != null)
            {
                UpdateCorrespondViewModel vm = new UpdateCorrespondViewModel()
                {
                    CorrespondId = cor.CorrespondId,
                    Cuando = cor.Cuando,
                    Remitente = cor.Remitente,
                    Mensaje = cor.Mensaje
                };
                return View(vm);
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //deletes the current message out of the database
        public async Task<IActionResult> DeleteCorrespond(int id, Correspondencia cor)
        {
            Correspondencia cores = await _uow.CorrespondenciaRepository.GetById(id);
            if (cores != null)
            {
                _uow.CorrespondenciaRepository.Delete(cores);
                await _uow.Save();
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("CorrespondOverview", new { id = cores.EmprId });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}