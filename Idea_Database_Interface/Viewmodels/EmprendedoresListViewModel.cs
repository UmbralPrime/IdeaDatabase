using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class EmprendedoresListViewModel
    {
        public IEnumerable<Emprendedores> Emprendedores { get; set;}
        public int FilterVal { get; set; }
        public string FilterText { get; set; }
    }
}
