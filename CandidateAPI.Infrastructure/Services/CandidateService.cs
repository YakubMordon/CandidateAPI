using CandidateAPI.Application.Interfaces.Mappers;
using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CandidateAPI.Infrastructure.Services;

public class CandidateService(
    ICandidateRepository candidateRepository,
    ICandidateMapper candidateMapper,
    IDistributedCache cache) : ICandidateService
{
    public async Task AddAsync(CandidateModel model)
    {
        var entity = candidateMapper.ModelToEntity(model);
        await candidateRepository.AddAsync(entity);

        await SetToCache(model);
    }

    public async Task DeleteAsync(CandidateModel model)
    {
        var entity = candidateMapper.ModelToEntity(model);
        await candidateRepository.DeleteAsync(entity);

        await cache.RemoveAsync(GetCacheKey(model.Email));
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

            await SetToCache(model);
        }
    }

    public async Task<CandidateModel?> GetByEmailAsync(string email)
    {
        var cacheKey = GetCacheKey(email);
        var cached = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            return JsonSerializer.Deserialize<CandidateModel>(cached);
        }

        var entity = await candidateRepository.GetByEmailAsync(email);
        if (entity is null) return null;

        var model = candidateMapper.EntityToModel(entity);
        await SetToCache(model);

        return model;
    }

    private string GetCacheKey(string email) => $"candidate:{email.ToLower()}";

    private async Task SetToCache(CandidateModel model)
    {
        var cacheKey = GetCacheKey(model.Email);
        var cacheValue = JsonSerializer.Serialize(model);

        await cache.SetStringAsync(cacheKey, cacheValue, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
    }
}