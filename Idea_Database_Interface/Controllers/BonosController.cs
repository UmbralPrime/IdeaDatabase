using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using System.Data;
using System.Drawing.Text;

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
            SelectList filterOptions = new SelectList(options);
            IEnumerable<Bonos> bonos = _uow.BonosRepository.GetAll().ToList().OrderBy(x => x.Nombre);

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
                    default:
                        break;
                }
                
            }
            if (filterDate != null)
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
            //And now it just works, i dont know how
            ModelState.Clear();
            return View(model);
        }
    }
}
