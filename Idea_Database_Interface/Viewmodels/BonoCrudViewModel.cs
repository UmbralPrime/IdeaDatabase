using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class BonoCrudViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string PrimerApellido { get; set; }
        public string? SegunodApellido { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string DNI { get; set; }
        public int NumeroDeBonos { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Correo { get; set; }
        public string? Direcction { get; set; }
        public string? CódigoPostal { get; set; }
        public string? Teléfono { get; set; }
        public string? TarjetaNum { get; set; }
        public string? NúmeroId { get; set; }
        public string? NúmeroId2 { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public TimeSpan Hours { get; set; }
    }
}
