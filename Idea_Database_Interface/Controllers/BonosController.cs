using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Services;
using Idea_Database_Interface.Viewmodels;
using IronXL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using System.Data;
using System.Drawing.Text;
using System.Globalization;
using System.IO;

namespace Idea_Database_Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class BonosController : Controller
    {
        private readonly IUnitOfWork _uow;
        public BonosController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult Index(string searchString, string filterSelect, int? page, DateTime? filterDate)
        {
            List<string> options = new();
            options.Add("Nombre");
            options.Add("Teléfono");
            options.Add("Correo");
            options.Add("DNI");
            options.Add("Localizador");
            SelectList filterOptions = new SelectList(options);
            IEnumerable<Bonos> bonos = _uow.BonosRepository.GetAll().OrderBy(x => x.Date);

            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        bonos = bonos.Where(x => x.Nombre.ToLower().Contains(searchString) || x.PrimerApellido.ToLower().Contains(searchString) || x.SegunodApellido.ToLower().Contains(searchString)).ToList();
                        break;
                    case "Teléfono":
                        bonos = bonos.Where(x => x.Teléfono.Contains(searchString)).ToList();
                        break;
                    case "Correo":
                        bonos = bonos.Where(x => x.Correo.ToLower().Contains(searchString)).ToList();
                        break;
                    case "DNI":
                        bonos = bonos.Where(x => x.DNI.ToLower().Contains(searchString)).ToList();
                        break;
                    case "Localizador":
                        bonos = bonos.Where(x => x.Localizador.ToLower().Contains(searchString)).ToList();
                        break;
                    default:
                        break;
                }

            }
            if (filterDate != null && filterDate != DateTime.MinValue)
                bonos = bonos.Where(x => x.Date.Date.Equals(filterDate.Value.Date)).ToList();
            //This is to prevent the searchbox from being erased
            ViewBag.SearchString = searchString;
            //Here you can change the amount of items on a page
            int pageSize = 10;
            int pageNumber = page ?? 1;
            //this is to check if the pagenumber isnt higher than the pagecount
            IPagedList testList = bonos.ToPagedList(1, pageSize);
            if (testList.PageCount < pageNumber)
                pageNumber = testList.PageCount;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            BonosListViewModel model = new BonosListViewModel()
            {
                Bonos = bonos.ToPagedList(pageNumber, pageSize),
                FilterOptions = filterOptions,
                PageCount = pageNumber,
                SearchedFilter = filterSelect,
                SearchedString = searchString,
                DateFilter = filterDate.GetValueOrDefault().Date,
            };
            //This resets the pagenumber for the next and previous page buttons
            //Without this you can only change the page once
            //And with this line of code it just works, i dont know how
            ModelState.Clear();
            return View(model);
        }
        public IActionResult Import()
        {
            FileUpload vm = new FileUpload();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile fileUpload)
        {
            IQueryable<Bonos> allDbBonos = _uow.BonosRepository.GetAll();
            try
            {
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    var ext = System.IO.Path.GetExtension(fileUpload.FileName);
                    if (ext != ".xls" && ext != ".xlsx")
                        return View(new FileUpload() { AlertMessage = "El archivo no tiene formato .xls o .xlsx" });
                    ExcelToDatabase toDb = new ExcelToDatabase(_uow);
                    await toDb.ImportToDatabaseAsync(fileUpload);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                FileUpload vmError = new FileUpload()
                {
                    AlertMessage = "Error al cargar el archivo"
                };
                return View(vmError);
            }
            FileUpload vm = new FileUpload()
            {
                AlertMessage = "Error al cargar el archivo"
            };
            return View(vm);
        }
        public IActionResult CreateBono()
        {
            BonoCrudViewModel vm = new BonoCrudViewModel();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBono(BonoCrudViewModel bono)
        {
            if (ModelState.IsValid)
            {
                Bonos toAdd = new Bonos()
                {
                    Nombre = bono.Nombre,
                    PrimerApellido = bono.PrimerApellido,
                    DNI = bono.DNI,
                    Date = bono.Date + bono.Hours,
                    Direcction = bono.Direcction,
                    NumeroDeBonos = bono.NumeroDeBonos,
                    Correo = bono.Correo,
                    CódigoPostal = bono.CódigoPostal,
                    NúmeroId = bono.NúmeroId,
                    NúmeroId2 = bono.NúmeroId2,
                    SegunodApellido = bono.SegunodApellido,
                    TarjetaNum = bono.TarjetaNum,
                    Teléfono = bono.Teléfono
                };
                _uow.BonosRepository.Create(toAdd);
                await _uow.Save();
                return RedirectToAction("Index");
            }
            return View(bono);
        }
        public async Task<IActionResult> Details(int id)
        {
            Bonos bono = await _uow.BonosRepository.GetById(id);
            BonosDetailsViewModel viewModel = new BonosDetailsViewModel() { Bonos = bono };
            return View(viewModel);
        }
        public async Task<IActionResult> UpdateBono(int id)
        {
            Bonos bono = await _uow.BonosRepository.GetById(id);
            BonoCrudViewModel viewModel = new BonoCrudViewModel()
            {
                Nombre = bono.Nombre,
                PrimerApellido = bono.PrimerApellido,
                DNI = bono.DNI,
                Date = bono.Date,
                Direcction = bono.Direcction,
                NumeroDeBonos = bono.NumeroDeBonos,
                Correo = bono.Correo,
                CódigoPostal = bono.CódigoPostal,
                NúmeroId = bono.NúmeroId,
                NúmeroId2 = bono.NúmeroId2,
                SegunodApellido = bono.SegunodApellido,
                TarjetaNum = bono.TarjetaNum,
                Teléfono = bono.Teléfono,
                Id = bono.Id
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBono(int id, BonoCrudViewModel bono)
        {
            if (ModelState.IsValid)
            {
                Bonos toUpdate = new Bonos()
                {
                    Nombre = bono.Nombre,
                    PrimerApellido = bono.PrimerApellido,
                    DNI = bono.DNI,
                    Date = bono.Date + bono.Hours,
                    Direcction = bono.Direcction,
                    NumeroDeBonos = bono.NumeroDeBonos,
                    Correo = bono.Correo,
                    CódigoPostal = bono.CódigoPostal,
                    NúmeroId = bono.NúmeroId,
                    NúmeroId2 = bono.NúmeroId2,
                    SegunodApellido = bono.SegunodApellido,
                    TarjetaNum = bono.TarjetaNum,
                    Teléfono = bono.Teléfono,
                    Id = id
                };
                _uow.BonosRepository.Update(toUpdate);
                await _uow.Save();
                return RedirectToAction("Index");
            }

            return View(bono);
        }
        public async Task<IActionResult> DeleteBono(int id)
        {
            Bonos toDel = await _uow.BonosRepository.GetById(id);
            _uow.BonosRepository.Delete(toDel);
            await _uow.Save();
            return RedirectToAction("Index");
        }
        public IActionResult ExportDatabase()
        {
            ExportDBDateViewModel vm = new ExportDBDateViewModel()
            {
                From = DateTime.Now.Date,
                Untill = DateTime.Now.Date
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult ExportDatabase(DateTime from, DateTime untill)
        {
            IQueryable<Bonos> bonos = _uow.BonosRepository.GetAll().Where(x=>x.Date>from&&x.Date<untill);
            ExportData export = new ExportData(bonos);
            return export.DownloadExcelDb();
        }
        [HttpPost]
        public async Task<IActionResult> ExportPdf(int id)
        {
            IQueryable<Bonos> bonos = _uow.BonosRepository.GetAll();
            ExportData export = new ExportData(bonos);
            return await export.PrintPdf(id);
        }
        public IActionResult DeleteAll()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAllConfirm()
        {
            IQueryable<Bonos> bonos = _uow.BonosRepository.GetAll();
            foreach (var bono in bonos)
            {
                _uow.BonosRepository.Delete(bono);
            }
            await _uow.Save();
            return RedirectToAction("Index");
        }
    }
}
