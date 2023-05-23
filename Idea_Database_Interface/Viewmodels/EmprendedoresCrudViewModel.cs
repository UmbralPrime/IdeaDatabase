using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class EmprendedoresCrudViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public TimeSpan FechaHora { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Teléfono { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
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
