using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;

namespace CandidateAPI.Application.Interfaces.Mappers;

public interface IMapper<TEntity, TModel> 
    where TEntity : BaseEntity 
    where TModel : BaseModel
{
    TEntity ModelToEntity(TModel model);
    TModel EntityToModel(TEntity entity);
}