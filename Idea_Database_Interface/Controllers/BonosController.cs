using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
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
            SelectList filterOptions = new SelectList(options);
            IEnumerable<Bonos> bonos = _uow.BonosRepository.GetAll().ToList().OrderBy(x => x.Date);

            //filters based on the input of the searchstring and the selected filter
            //if the search string is empty it will just show all the companies
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                switch (filterSelect)
                {
                    case "Nombre":
                        bonos = bonos.Where(x => x.Nombre.ToLower().Contains(searchString)).ToList();
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
                    default:
                        break;
                }

            }
            if (filterDate != null && filterDate != DateTime.MinValue)
                bonos = bonos.Where(x => x.Date.Equals(filterDate.Value.Date)).ToList();
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
                DateFilter = filterDate.GetValueOrDefault().Date
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
        public async Task<IActionResult> Import(IFormFile fileUpload, int dateYear)
        {
            IQueryable<Bonos> allDbBonos = _uow.BonosRepository.GetAll();
            IEnumerable<Bonos> DbBonos = allDbBonos;
            try
            {
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    //if(fileUpload.FileName==".xls"|| fileUpload.FileName==".xlsx")
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUpload.FileName);
                    using (var stream = System.IO.File.Create(filePath, 4096, FileOptions.DeleteOnClose))
                    {
                        await fileUpload.CopyToAsync(stream);
                        WorkBook wb = WorkBook.FromStream(stream);
                        DataSet dataSet = wb.ToDataSet();
                        foreach (DataTable table in dataSet.Tables)
                        {
                            DateTime tableTime = DateTime.Parse($"{table.TableName} {dateYear}", new CultureInfo("es-ES"));
                            foreach (DataRow row in table.Rows)
                            {
                                if (row[1].ToString() != "Hora" && row[4].ToString() != "TOTAl" && !string.IsNullOrEmpty(row[3].ToString()))
                                {
                                    TimeSpan hour = DateTime.Parse(row[1].ToString()).TimeOfDay;
                                    DateTime usedTime = tableTime + hour;
                                    int numeroBonos = 0;
                                    int.TryParse(row[6].ToString(), out numeroBonos);
                                    Bonos bonos = new Bonos()
                                    {
                                        Date = usedTime,
                                        Direcction = row[9].ToString(),
                                        DNI = row[5].ToString(),
                                        Nombre = row[2].ToString(),
                                        PrimerApellido = row[3].ToString(),
                                        SegunodApellido = row[4].ToString(),
                                        NumeroDeBonos = numeroBonos,
                                        Correo = row[8].ToString(),
                                        CódigoPostal = row[10].ToString(),
                                        Teléfono = row[11].ToString(),
                                        TarjetaNum = row[12].ToString(),
                                        NúmeroId = row[13].ToString(),
                                        NúmeroId2 = row[14].ToString()
                                    };
                                    if (await bonos.EqualsAsync(allDbBonos) == false)
                                    {
                                        _uow.BonosRepository.Create(bonos);
                                        await _uow.Save();
                                    }
                                }
                            }
                        }
                        stream.Dispose();
                    }
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
                    Date = bono.Date,
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
    }
}
