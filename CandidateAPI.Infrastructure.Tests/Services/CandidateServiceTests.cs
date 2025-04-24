using System.Text;
using System.Text.Json;
using CandidateAPI.Application.Interfaces.Mappers;
using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;
using CandidateAPI.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;

namespace CandidateAPI.Infrastructure.Tests.Services;

public class CandidateServiceTests
{
    private readonly Mock<ICandidateRepository> _mockCandidateRepository;
    private readonly Mock<ICandidateMapper> _mockCandidateMapper;
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly CandidateService _candidateService;

    public CandidateServiceTests()
    {
        _mockCandidateRepository = new Mock<ICandidateRepository>();
        _mockCandidateMapper = new Mock<ICandidateMapper>();
        _mockCache = new Mock<IDistributedCache>();
        _candidateService = new CandidateService(_mockCandidateRepository.Object, _mockCandidateMapper.Object, _mockCache.Object);
    }

    private byte[] Serialize<T>(T obj) =>
        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

    private T Deserialize<T>(byte[] data) =>
        JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));

    [Fact]
    public async Task AddAsync_ShouldAddCandidate_AndCacheIt()
    {
        var model = new CandidateModel { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        var entity = new Candidate { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        _mockCandidateMapper.Setup(m => m.ModelToEntity(model)).Returns(entity);
        _mockCandidateRepository.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default));

        await _candidateService.AddAsync(model);

        _mockCandidateRepository.Verify(r => r.AddAsync(entity), Times.Once);
        _mockCache.Verify(c => c.SetAsync(
            It.Is<string>(k => k.Contains("candidate:john.doe@example.com")),
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCandidate_AndRemoveFromCache()
    {
        var model = new CandidateModel { Email = "john.doe@example.com" };
        var entity = new Candidate { Email = "john.doe@example.com" };

        _mockCandidateMapper.Setup(m => m.ModelToEntity(model)).Returns(entity);
        _mockCandidateRepository.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
        _mockCache.Setup(c => c.RemoveAsync(It.IsAny<string>(), default));

        await _candidateService.DeleteAsync(model);

        _mockCandidateRepository.Verify(r => r.DeleteAsync(entity), Times.Once);
        _mockCache.Verify(c => c.RemoveAsync("candidate:john.doe@example.com", default), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCandidates()
    {
        var entities = new List<Candidate> { new() { Email = "john.doe@example.com", FirstName = "John", LastName = "Doe" } };

        _mockCandidateRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mockCandidateMapper.Setup(m => m.EntityToModel(It.IsAny<Candidate>()))
            .Returns((Candidate c) => new CandidateModel { Email = c.Email, FirstName = c.FirstName, LastName = c.LastName });

        var result = await _candidateService.GetAllAsync();

        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCandidate_AndCacheIt()
    {
        var model = new CandidateModel { Email = "john.doe@example.com" };
        var entity = new Candidate { Email = "john.doe@example.com" };

        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(model.Email)).ReturnsAsync(entity);
        _mockCandidateMapper.Setup(m => m.ModelToEntity(model)).Returns(entity);
        _mockCandidateRepository.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

        await _candidateService.UpdateAsync(model);

        _mockCandidateRepository.Verify(r => r.UpdateAsync(entity), Times.Once);
        _mockCache.Verify(
            c => c.SetAsync("candidate:john.doe@example.com", It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdate_WhenCandidateNotFound()
    {
        var model = new CandidateModel { Email = "john.doe@example.com" };
        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(model.Email)).ReturnsAsync((Candidate?)null);

        await _candidateService.UpdateAsync(model);

        _mockCandidateRepository.Verify(r => r.UpdateAsync(It.IsAny<Candidate>()), Times.Never);
        _mockCache.Verify(
            c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default),
            Times.Never);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCandidate_FromCache()
    {
        var email = "john.doe@example.com";
        var model = new CandidateModel { Email = email };
        var serialized = Serialize(model);

        _mockCache.Setup(c => c.GetAsync("candidate:john.doe@example.com", default)).ReturnsAsync(serialized);

        var result = await _candidateService.GetByEmailAsync(email);

        Assert.NotNull(result);
        Assert.Equal(email, result?.Email);
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCandidate_FromRepository_AndCacheIt()
    {
        var email = "john.doe@example.com";
        var entity = new Candidate { Email = email };
        var model = new CandidateModel { Email = email };

        _mockCache.Setup(c => c.GetAsync("candidate:john.doe@example.com", default)).ReturnsAsync((byte[]?)null);
        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(entity);
        _mockCandidateMapper.Setup(m => m.EntityToModel(entity)).Returns(model);

        var result = await _candidateService.GetByEmailAsync(email);

        Assert.NotNull(result);
        _mockCache.Verify(
            c => c.SetAsync("candidate:john.doe@example.com", It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_IfNotFound()
    {
        var email = "nonexistent@example.com";
        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), default)).ReturnsAsync((byte[]?)null);
        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((Candidate?)null);

        var result = await _candidateService.GetByEmailAsync(email);

        Assert.Null(result);
    }
}