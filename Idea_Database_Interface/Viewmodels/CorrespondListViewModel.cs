using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class CorrespondListViewModel
    {
        public int EmprId { get; set; }
        public Empresa Empresa { get; set; }
        public List<Correspondencia> Correspondencias { get; set; }
    }
}
