using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Idea_Database_Interface.Viewmodels
{
    public class EmprendedoresCrudViewModel
    {
        public int? Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan FechaHora { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Teléfono { get; set; }
        public string Email { get; set; }
        public string? MotivoDeLaConsulto { get; set; }
        public string? Incidencias { get; set; }
        public string? Observaciones { get; set; }
        public bool PlanViabilidad { get; set; }
        public bool Terminado { get; set; }
        public MultiSelectList? AllCategorias { get; set; }
        public List<EmprendedoresCategoría>? Categorias { get; set; }
        public int[]? SelectedCategorias { get; set; }
    }
}
