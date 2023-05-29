using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    public class Targeta
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string IAE { get; set; }
        public string CódigoFUC { get; set;}
        public string TipoDeVia { get; set; }
        public string Dirección { get; set; }
        public string? TeléfonoFijo { get; set; }
        public string? TeléfonoMóvil { get; set; }
        public string? Correo { get; set; }
    }
}
