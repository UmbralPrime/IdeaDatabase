using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    public class Emprendedores
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Teléfono { get; set; }
        public string Email { get; set; }
        public string? MotivoDeLaConsulto { get; set; }
        public string? Incidencias { get; set; }
        public bool PlanViabilidad { get; set; }
        public IEnumerable<EmprendedoresCategoría> Categorías { get; set; }

    }
}
