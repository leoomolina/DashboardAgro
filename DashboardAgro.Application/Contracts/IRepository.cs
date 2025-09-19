namespace DashboardAgro.Application.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(object id);
        IQueryable Query();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SaveChangesAsync();
    }
}
