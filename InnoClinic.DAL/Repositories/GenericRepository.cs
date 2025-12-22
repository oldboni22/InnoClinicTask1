using InnoClinic.DAL.Entities;

namespace InnoClinic.DAL.Repositories;

public sealed class GenericRepository<T>(TaskDbContext context) : IGenericRepository<T> where T : EntityBase
{
    public Task<T> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetByConditionAsync(Func<T, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}