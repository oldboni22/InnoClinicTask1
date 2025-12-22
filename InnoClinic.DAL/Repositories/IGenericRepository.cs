using InnoClinic.DAL.Entities;

namespace InnoClinic.DAL.Repositories;

public interface IGenericRepository<T> where T : EntityBase
{
    Task<T> GetByIdAsync(Guid id);
    
    Task<IEnumerable<T>> GetByConditionAsync(Func<T, bool> predicate);
    
    Task<T> UpdateAsync(T entity);
    
    Task<T> CreateAsync(T entity);
    
    Task<bool> DeleteAsync(Guid id);
}
