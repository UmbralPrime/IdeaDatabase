using Idea_Database_Interface.Data.Repository.Interfaces;
using Idea_Database_Interface.Models;

namespace Idea_Database_Interface.Data.Repository
{
    public class BonosRepository : IBonosRepository
    {
        private readonly IdeaDBContext _context;
        public BonosRepository(IdeaDBContext context)
        {
            _context = context;
        }

        public IQueryable<Bonos> GetAll()
        {
            return _context.Set<Bonos>();
        }

        public async Task<Bonos> GetById(int id)
        {
            return await _context.Set<Bonos>().FindAsync(id);
        }

        public void Create(Bonos entity)
        {
            try
            {
                _context.Set<Bonos>().Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message);
            }
        }

        public void Update(Bonos entity)
        {
            //Untrack all the entities. Sometimes it gives trouble when an entity is still
            //tracked and you try to update it
            _context.ChangeTracker.Clear();
            _context.Set<Bonos>().Update(entity);
        }

        public void Delete(Bonos entity)
        {
            _context.Set<Bonos>().Remove(entity);
        }

    }
}
