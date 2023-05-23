using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Viewmodels
{
    public class CategoriaCrudViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public int Year { get; set; }
        public bool IsActive { get; set; }
        public SelectList? Years { get; set; }
    }
}
