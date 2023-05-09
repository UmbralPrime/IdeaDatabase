using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class CategoriasListViewModel
    {
        public List<Categoría> Categorias { get; set; }
        public List<EmprendedoresCategoría> EmprendedoresCategorias { get; set; }
        public List<CatYear> CatYears { get; set; }
    }
}
