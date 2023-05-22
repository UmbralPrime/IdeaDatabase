using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Idea_Database_Interface.Models
{
    public class Bonos
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegunodApellido { get; set; }
        public string DNI { get; set; }
        public int NumeroDeBonos { get; set; }
        public string Correo { get; set; }
        public string? Direcction { get; set; }
        public string? CódigoPostal { get; set; }
        public string? Teléfono { get; set; }
        public string? TarjetaNum { get; set; }
        public string? NúmeroId { get; set; }
        public string? NúmeroId2 { get; set; }
        public string Localizador { get; set; }
        public string Localidad { get; set; }
        public async Task<bool> EqualsAsync(object? obj)
        {
            if (obj == null) return false;
            bool same = false;
            IQueryable<Bonos> queryObj = obj as IQueryable<Bonos>;
            if(queryObj.Count()==0)
                return false;
            await queryObj.ForEachAsync(item =>
            {
                if (!same)
                    same = item.Date == this.Date && item.Nombre == this.Nombre
                    && item.PrimerApellido == this.PrimerApellido;
            });
            return same;
        }
    }
}
