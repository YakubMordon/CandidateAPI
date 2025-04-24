using CandidateAPI.Domain.Entities;
using CandidateAPI.Infrastructure.Repositories;
using CandidateAPI.Infrastructure.Tests.Fakers;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace CandidateAPI.Infrastructure.Tests.Repositories;

public class CandidateRepositoryTests : IDisposable, IAsyncDisposable
{
    private DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly ApplicationDbContext _context;
    private readonly CandidateRepository _candidateRepository;
    private readonly CandidateFaker _candidateFaker;

    public CandidateRepositoryTests()
    {
        _candidateFaker = new CandidateFaker();

        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_candidates")
            .Options;

        _context = new ApplicationDbContext(_dbContextOptions);
        _candidateRepository = new CandidateRepository(_context);
    }

    [Fact]
    public async Task AddAsync_DataIsCorrect_ShouldAddCandidate()
    {
        // Arrange
        var entity = _candidateFaker.Generate();

        // Act
        await _candidateRepository.AddAsync(entity);
        await _context.SaveChangesAsync();  // Збереження змін

        // Assert
        _context.Candidates.Should().Contain(entity);  // Перевірка, чи зберігся кандидат в базі

        // Cleanup
        await _candidateRepository.DeleteAsync(entity);
    }


    [Fact]
    public async Task AddAsync_DataIsNotCorrect_ShouldNotAddCandidate()
    {
        // Arrange
        var entity = new Candidate
        {
            Email = "invalid-email",  // Invalid email format
            FirstName = "1",
            LastName = "1"
        };

        // Act
        var adding = async () => await _candidateRepository.AddAsync(entity);

        // Assert
        await adding.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task DeleteAsync_CandidateExists_ShouldDeleteCandidate()
    {
        // Arrange
        var entity = _candidateFaker.Generate();
        await _candidateRepository.AddAsync(entity);

        // Act
        await _candidateRepository.DeleteAsync(entity);

        // Assert
        _context.Candidates.Should().NotContain(entity);
    }

    [Fact]
    public async Task DeleteAsync_CandidateNotExists_ShouldNotDeleteCandidate()
    {
        // Arrange
        var entity = _candidateFaker.Generate();

        // Act
        var removal = async () => await _candidateRepository.DeleteAsync(entity);

        // Assert
        await removal.Should().NotThrowAsync();  // No exception should be thrown
        _context.Candidates.Should().NotContain(entity);  // Should not affect the context
    }

    [Fact]
    public async Task UpdateAsync_DataIsCorrect_ShouldUpdateCandidate()
    {
        // Arrange
        var entity = _candidateFaker.Generate();
        await _candidateRepository.AddAsync(entity);  // Додаємо сутність в базу даних

        // Оновлюємо властивість
        entity.LastName = "NEW_LAST_NAME";

        // Act
        _context.Candidates.Attach(entity);
        await _candidateRepository.UpdateAsync(entity);  // Оновлюємо

        // Assert
        var updatedEntity = await _candidateRepository.GetByEmailAsync(entity.Email);
        updatedEntity.Should().NotBeNull();
        updatedEntity.LastName.Should().Be("NEW_LAST_NAME");

        // Cleanup
        await _candidateRepository.DeleteAsync(entity);  // Очищаємо
    }


    [Fact]
    public async Task GetAllAsync_ShouldGetAllCandidates()
    {
        // Arrange
        var list = _candidateFaker.Generate(10);
        _context.Candidates.AddRange(list);
        await _context.SaveChangesAsync();

        // Act
        var actual = await _candidateRepository.GetAllAsync();

        // Assert
        actual.Should().BeEquivalentTo(list);

        // Cleanup
        _context.Candidates.RemoveRange(list);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetByEmailAsync_CandidateExists_ShouldReturnCandidate()
    {
        // Arrange
        var entity = _candidateFaker.Generate();
        await _candidateRepository.AddAsync(entity);

        // Act
        var actual = await _candidateRepository.GetByEmailAsync(entity.Email);

        // Assert
        actual.Should().BeEquivalentTo(entity);

        // Cleanup
        await _candidateRepository.DeleteAsync(entity);
    }

    [Fact]
    public async Task GetByEmailAsync_CandidateNotExists_ShouldReturnNull()
    {
        // Act
        var actual = await _candidateRepository.GetByEmailAsync("randomEmail");

        // Assert
        actual.Should().BeNull();
    }

    public void Dispose() => _context?.Dispose();

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
