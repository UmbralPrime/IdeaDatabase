using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    //This is the class for the messages that have been sent to a company
    public class Correspondencia
    {
        [Key]
        public int CorrespondId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Cuando { get; set; }
        public string Remitente { get; set; }
        public int EmprId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
