using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(ApplicationDbContext context)
    where TEntity : BaseEntity
{
    public virtual async Task AddAsync(TEntity entity)
    {
        await context.AddAsync(entity);

        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        context.Remove(entity);

        await context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Attach(entity);

        await context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await context.Set<TEntity>().ToListAsync();
}