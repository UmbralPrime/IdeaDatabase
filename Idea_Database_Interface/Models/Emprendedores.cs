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
        public string? Observaciones { get; set; }
        public bool PlanViabilidad { get; set; }   
        public bool Terminado { get; set; }
        public ICollection <EmprendedoresCategoría>? Categorías { get; set; }
        public List<Categoría> NamedCategories { get; set; }

    }
}
