using CandidateAPI.Domain.Entities;

namespace CandidateAPI.Application.Interfaces.Repositories;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync();
}