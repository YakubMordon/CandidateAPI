using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;

namespace CandidateAPI.Application.Interfaces.Services;

public interface IApplicationService<TModel>
    where TModel : BaseModel
{
    Task AddAsync(TModel model);
    Task DeleteAsync(TModel model);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task UpdateAsync(TModel model);
}