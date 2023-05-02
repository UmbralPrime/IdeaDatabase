using Idea_Database_Interface.Data.Repository.Interfaces;
using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Empresa> EmpresaRepository { get; }
        IGenericRepository<Correspondencia> CorrespondenciaRepository { get; }
        IGenericRepository<Emprendedores> EmprendedoresRepository { get; }
        IGenericRepository<Categoría> CategoriaRepository { get; }
        Task Save();
    }
}
