using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class CreateCorrespondViewModel
    {
        public int EmprId { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Mensaje { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public DateTime Cuando { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Remitente { get; set; }
    }
}
