namespace RagnarApp.Application.Abstract
{
    public interface IGenericRepository<TEntity>
    {
        // Get Data

        Task<List<TEntity>> GetAll();
        Task<TEntity> GetByIdAsync(int id);

        //Create

        Task Create(TEntity entity);
        Task CreateRange(IEnumerable<TEntity> entities);

        //Update

        Task Update(TEntity entity);

        //Delete

        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        //Save

        Task<bool> SaveAsync();
    }
}
