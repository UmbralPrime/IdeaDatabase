using Idea_Database_Interface.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Idea_Database_Interface.Data.Repository
{
    public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity: class
    {
        private readonly IdeaDBContext _context;
        public GenericRepository(IdeaDBContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public void Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message);
            }
        }

        public void Update(TEntity entity)
        {
            //Untrack all the entities. Sometimes it gives trouble when an entity is still
            //tracked and you try to update it
            _context.ChangeTracker.Clear();
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}
