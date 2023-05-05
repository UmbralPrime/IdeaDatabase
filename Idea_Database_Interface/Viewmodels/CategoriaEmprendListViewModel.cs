using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class CategoriaEmprendListViewModel
    {
        public Categoría Categoría { get; set; }
        public IEnumerable<Emprendedores> Emprendedores { get; set; }
        public int FilterVal { get; set; }
        public string FilterText { get; set; }
    }
}
