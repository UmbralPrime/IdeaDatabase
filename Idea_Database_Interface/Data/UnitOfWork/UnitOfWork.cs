using Idea_Database_Interface.Data.Repository;
using Idea_Database_Interface.Data.Repository.Interfaces;
using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdeaDBContext _context;
        private IGenericRepository<Empresa> empresaRepo;
        private IGenericRepository<Correspondencia> correspondenciaRepo;
        private IGenericRepository<Emprendedores> emprendedoresRepo;
        private IGenericRepository<Categoría> categoriaRepo;
        public UnitOfWork(IdeaDBContext dBContext)
        {
            _context = dBContext;
        }
        public IGenericRepository<Empresa> EmpresaRepository
        {
            get
            {
                if (empresaRepo == null)
                {
                    this.empresaRepo = new GenericRepository<Empresa>(_context);
                }
                return empresaRepo;
            }
        }

        public IGenericRepository<Correspondencia> CorrespondenciaRepository
        {
            get
            {
                if (correspondenciaRepo == null)
                    this.correspondenciaRepo = new GenericRepository<Correspondencia>(_context);
                return correspondenciaRepo;
            }
        }
        public IGenericRepository<Emprendedores> EmprendedoresRepository
        {
            get
            {
                if (emprendedoresRepo == null)
                {
                    this.emprendedoresRepo = new GenericRepository<Emprendedores>(_context);
                }
                return emprendedoresRepo;
            }
        }
        public IGenericRepository<Categoría> CategoriaRepository
        {
            get
            {
                if (categoriaRepo == null)
                    this.categoriaRepo = new GenericRepository<Categoría>(_context);
                return categoriaRepo;
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
