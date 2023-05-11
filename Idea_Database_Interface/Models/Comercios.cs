using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    public class Comercios
    {
        [Key] 
        public int Id { get; set; }
        public string NombreComercial { get; set; }
        public string IAE { get; set; }
        public string? CódigoFUC { get; set; }
        public string? Calle { get; set; }
        public string? Número { get; set; }
        public int? CódigoPostal { get; set; }
        public string? Provincia { get; set; }
        public string? Municipio { get; set; }
        public string? Localidad { get; set; }
        public string? TeléfonoFijo { get; set; }
        public string Móvi { get; set; }
        public string Email { get; set; }
        public string CIF { get; set; }
        public string? Contacto { get; set; }
    }
}
