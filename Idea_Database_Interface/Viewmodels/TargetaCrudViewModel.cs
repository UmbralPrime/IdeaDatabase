using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class TargetaCrudViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string IAE { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string CódigoFUC { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string TipoDeVia { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Dirección { get; set; }
        public string? TeléfonoFijo { get; set; }
        public string? TeléfonoMóvil { get; set; }
        public string? Correo { get; set; }
    }
}
