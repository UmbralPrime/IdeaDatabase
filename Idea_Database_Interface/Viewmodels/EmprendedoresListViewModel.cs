using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Idea_Database_Interface.Viewmodels
{
    public class EmprendedoresListViewModel
    {
        public IEnumerable<Emprendedores> Emprendedores { get; set;}
        public SelectList FilterOptions { get; set; }
    }
}
