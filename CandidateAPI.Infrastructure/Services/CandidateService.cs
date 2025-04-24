using CandidateAPI.Application.Interfaces.Mappers;
using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Domain.Models;

namespace CandidateAPI.Infrastructure.Services;

public class CandidateService(ICandidateRepository candidateRepository, ICandidateMapper candidateMapper) : ICandidateService
{
    public async Task AddAsync(CandidateModel model)
    {
        var entity = candidateMapper.ModelToEntity(model);

        await candidateRepository.AddAsync(entity);
    }

    public async Task DeleteAsync(CandidateModel model)
    {
        var entity = candidateMapper.ModelToEntity(model);

        await candidateRepository.DeleteAsync(entity);
    }

    public async Task<IEnumerable<CandidateModel>> GetAllAsync()
    {
        var entities = await candidateRepository.GetAllAsync();
        return entities.Select(candidateMapper.EntityToModel);
    }

    public async Task UpdateAsync(CandidateModel model)
    {
        var entity = await candidateRepository.GetByEmailAsync(model.Email);

        if (entity is not null)
        {
            var mappedEntity = candidateMapper.ModelToEntity(model);

            await candidateRepository.UpdateAsync(mappedEntity);
        }
    }

    public async Task<CandidateModel?> GetByEmailAsync(string email)
    {
        CandidateModel? result = null;

        var entity = await candidateRepository.GetByEmailAsync(email);

        if (entity is not null)
        {
            result = candidateMapper.EntityToModel(entity);
        }

        return result;
    }
}