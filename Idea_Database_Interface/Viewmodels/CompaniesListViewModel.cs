using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Idea_Database_Interface.Viewmodels
{
    public class CompaniesListViewModel
    {
        public IEnumerable<Empresa> Empresas { get; set; }
        public SelectList FilterOptions { get; set; }
    }
}
