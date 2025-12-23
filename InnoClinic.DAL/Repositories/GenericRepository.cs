using System.Linq.Expressions;
using InnoClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.DAL.Repositories;

public sealed class GenericRepository<T>(TaskDbContext context) : IGenericRepository<T> where T : EntityBase
{
    public Task<T?> GetByIdAsync(Guid id)
    {
        return context
            .Set<T>()
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        return await context
            .Set<T>()
            .Where(predicate)
            .ToListAsync<T>();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        var updatedEntity = context.Set<T>().Update(entity);

        await context.SaveChangesAsync();
        
        return updatedEntity.Entity;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var createdEntity = await context.Set<T>().AddAsync(entity);
        
        await context.SaveChangesAsync();
        
        return createdEntity.Entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var target = await GetByIdAsync(id);
        
        if (target is null)
        {
            return false;
        }

        context.Set<T>().Remove(target);
        await context.SaveChangesAsync();
        
        return true;
    }
}