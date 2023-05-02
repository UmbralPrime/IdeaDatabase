using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Viewmodels
{
    public class CorrespondDetailsViewModel
    {
        public int CorrespondId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Cuando { get; set; }
        public string Remitente { get; set; }
        public int EmprId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
