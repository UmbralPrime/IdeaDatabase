using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class ComerciosListViewModel
    {
        public IQueryable<Comercios> Comercios { get; set; }
        public int FilterVal { get; set; }
        public string FilterText { get; set; }
    }
}
