using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Idea_Database_Interface.Viewmodels
{
    public class CategoriaCrudViewModel
    {
        public string Nombre { get; set; }
        public int Year { get; set; }
        public SelectList? Years { get; set; }
    }
}
