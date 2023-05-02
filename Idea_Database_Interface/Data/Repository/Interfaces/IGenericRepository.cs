namespace Idea_Database_Interface.Data.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(int id);

        void Create(TEntity entity);
        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
