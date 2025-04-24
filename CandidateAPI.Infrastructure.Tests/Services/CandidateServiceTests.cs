using CandidateAPI.Application.Interfaces.Mappers;
using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;
using CandidateAPI.Infrastructure.Services;
using Moq;

namespace CandidateAPI.Infrastructure.Tests.Services;

public class CandidateServiceTests
{
    private readonly Mock<ICandidateRepository> _mockCandidateRepository;
    private readonly Mock<ICandidateMapper> _mockCandidateMapper;
    private readonly CandidateService _candidateService;

    public CandidateServiceTests()
    {
        _mockCandidateRepository = new Mock<ICandidateRepository>();
        _mockCandidateMapper = new Mock<ICandidateMapper>();
        _candidateService = new CandidateService(_mockCandidateRepository.Object, _mockCandidateMapper.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCandidate()
    {
        // Arrange
        var candidateModel = new CandidateModel { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };
        var candidateEntity = new Candidate { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };

        _mockCandidateMapper.Setup(m => m.ModelToEntity(candidateModel)).Returns(candidateEntity);
        _mockCandidateRepository.Setup(r => r.AddAsync(candidateEntity)).Returns(Task.CompletedTask);

        // Act
        await _candidateService.AddAsync(candidateModel);

        // Assert
        _mockCandidateMapper.Verify(m => m.ModelToEntity(candidateModel), Times.Once);
        _mockCandidateRepository.Verify(r => r.AddAsync(candidateEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCandidate()
    {
        // Arrange
        var candidateModel = new CandidateModel { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };
        var candidateEntity = new Candidate { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };

        _mockCandidateMapper.Setup(m => m.ModelToEntity(candidateModel)).Returns(candidateEntity);
        _mockCandidateRepository.Setup(r => r.DeleteAsync(candidateEntity));

        // Act
        await _candidateService.DeleteAsync(candidateModel);

        // Assert
        _mockCandidateMapper.Verify(m => m.ModelToEntity(candidateModel), Times.Once);
        _mockCandidateRepository.Verify(r => r.DeleteAsync(candidateEntity), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCandidates()
    {
        // Arrange
        var candidateModels = new List<CandidateModel> { new CandidateModel { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" } };
        var candidateEntities = new List<Candidate> { new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" } };

        _mockCandidateRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(candidateEntities);
        _mockCandidateMapper.Setup(m => m.EntityToModel(It.IsAny<Candidate>())).Returns((Candidate entity) =>
            new CandidateModel
            {
                FirstName = entity.FirstName, LastName = entity.LastName, Email = entity.Email,
                FreeTextComment = entity.FreeTextComment
            });

        // Act
        var result = await _candidateService.GetAllAsync();

        // Assert
        Assert.Equal(candidateModels.Count, result.Count());
        _mockCandidateRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockCandidateMapper.Verify(m => m.EntityToModel(It.IsAny<Candidate>()), Times.Exactly(candidateModels.Count));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCandidate_WhenEntityExists()
    {
        // Arrange
        var candidateModel = new CandidateModel
            { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };
        var candidateEntity = new Candidate
            { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };

        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(candidateModel.Email)).ReturnsAsync(candidateEntity);
        _mockCandidateMapper.Setup(m => m.ModelToEntity(candidateModel)).Returns(candidateEntity);
        _mockCandidateRepository.Setup(r => r.UpdateAsync(candidateEntity)).Returns(Task.CompletedTask);

        // Act
        await _candidateService.UpdateAsync(candidateModel);

        // Assert
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(candidateModel.Email), Times.Once);
        _mockCandidateMapper.Verify(m => m.ModelToEntity(candidateModel), Times.Once);
        _mockCandidateRepository.Verify(r => r.UpdateAsync(candidateEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateCandidate_WhenEntityDoesNotExist()
    {
        // Arrange
        var candidateModel = new CandidateModel
            { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };

        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(candidateModel.Email)).ReturnsAsync((Candidate?)null);

        // Act
        await _candidateService.UpdateAsync(candidateModel);

        // Assert
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(candidateModel.Email), Times.Once);
        _mockCandidateMapper.Verify(m => m.ModelToEntity(candidateModel), Times.Never);
        _mockCandidateRepository.Verify(r => r.UpdateAsync(It.IsAny<Candidate>()), Times.Never);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCandidate_WhenEmailExists()
    {
        // Arrange
        var candidateModel = new CandidateModel { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext"};
        var candidateEntity = new Candidate { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", FreeTextComment = "Sometext" };

        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(candidateModel.Email)).ReturnsAsync(candidateEntity);
        _mockCandidateMapper.Setup(m => m.EntityToModel(candidateEntity)).Returns(candidateModel);

        // Act
        var result = await _candidateService.GetByEmailAsync(candidateModel.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(candidateModel.Email, result?.Email);
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(candidateModel.Email), Times.Once);
        _mockCandidateMapper.Verify(m => m.EntityToModel(candidateEntity), Times.Once);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        // Arrange
        const string email = "nonexistent@example.com";

        _mockCandidateRepository.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((Candidate?)null);

        // Act
        var result = await _candidateService.GetByEmailAsync(email);

        // Assert
        Assert.Null(result);
        _mockCandidateRepository.Verify(r => r.GetByEmailAsync(email), Times.Once);
    }
}