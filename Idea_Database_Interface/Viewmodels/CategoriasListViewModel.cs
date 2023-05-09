using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class CategoriasListViewModel
    {
        public IEnumerable<Categoría> Categorias { get; set; }
        public IEnumerable<EmprendedoresCategoría> EmprendedoresCategorias { get; set; }
    }
}
