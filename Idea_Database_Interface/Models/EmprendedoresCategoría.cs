using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    public class EmprendedoresCategoría
    {
        [Key]
        public int Id { get; set; }
        public int IdCategoría { get; set; }
        public Categoría Categoría { get; set; }
        public int IdEmprendedores { get; set; }
        public Emprendedores Emprendedores { get; set; }
    }
}
