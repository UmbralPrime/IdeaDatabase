using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class UpdateCorrespondViewModel
    {
        public int CorrespondId { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Mensaje { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public DateTime Cuando { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Remitente { get; set; }
        public int EmprId { get; set; }
    }
}
