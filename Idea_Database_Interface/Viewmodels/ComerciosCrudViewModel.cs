using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class ComerciosCrudViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NombreComercial { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string IAE { get; set; }
        public string? CódigoFUC { get; set; }
        public string? Calle { get; set; }
        public string? Número { get; set; }
        public int? CódigoPostal { get; set; }
        public string? Provincia { get; set; }
        public string? Municipio { get; set; }
        public string? Localidad { get; set; }
        public string? TeléfonoFijo { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Móvi { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string CIF { get; set; }
        public string? Contacto { get; set; }
    }
}
