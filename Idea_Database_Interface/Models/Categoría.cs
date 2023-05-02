using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    public class Categoría
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public IEnumerable<Emprendedores> Emprendedoreses { get; set; }
        public IEnumerable<EmprendedoresCategoría> Emprendedores { get; set; }
        public override string ToString()
        {
            return this.Nombre;
        }
    }
}
