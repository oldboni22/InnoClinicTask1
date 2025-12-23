using System.Linq.Expressions;
using InnoClinic.DAL.Entities;

namespace InnoClinic.DAL.Repositories;

public interface IGenericRepository<T> where T : EntityBase
{
    Task<T> GetByIdAsync(Guid id);
    
    Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
    
    Task<T> UpdateAsync(T entity);
    
    Task<T> CreateAsync(T entity);
    
    Task<bool> DeleteAsync(Guid id);
}
