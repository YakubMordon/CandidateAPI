using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;
using CandidateAPI.Infrastructure.Mappers;

namespace CandidateAPI.Infrastructure.Tests.Mappers;

public class CandidateMapperTests
{
    private readonly CandidateMapper _candidateMapper = new();

    [Fact]
    public void ModelToEntity_ShouldMapCandidateModelToCandidateEntity()
    {
        // Arrange
        var candidateModel = new CandidateModel
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "123-456-7890",
            Email = "johndoe@example.com",
            CallTimeInterval = "09:00 - 10:00",
            LinkedInProfileUrl = "https://www.linkedin.com/in/johndoe",
            GitHubProfileUrl = "https://github.com/johndoe",
            FreeTextComment = "Experienced developer."
        };

        // Act
        var result = _candidateMapper.ModelToEntity(candidateModel);

        // Assert
        Assert.Equal(candidateModel.FirstName, result.FirstName);
        Assert.Equal(candidateModel.LastName, result.LastName);
        Assert.Equal(candidateModel.PhoneNumber, result.PhoneNumber);
        Assert.Equal(candidateModel.Email, result.Email);
        Assert.Equal(candidateModel.CallTimeInterval, result.CallTimeInterval);
        Assert.Equal(candidateModel.LinkedInProfileUrl, result.LinkedInProfileUrl);
        Assert.Equal(candidateModel.GitHubProfileUrl, result.GitHubProfileUrl);
        Assert.Equal(candidateModel.FreeTextComment, result.FreeTextComment);
    }

    [Fact]
    public void EntityToModel_ShouldMapCandidateEntityToCandidateModel()
    {
        // Arrange
        var candidateEntity = new Candidate
        {
            FirstName = "Jane",
            LastName = "Smith",
            PhoneNumber = "987-654-3210",
            Email = "janesmith@example.com",
            CallTimeInterval = "10:00 - 11:00",
            LinkedInProfileUrl = "https://www.linkedin.com/in/janesmith",
            GitHubProfileUrl = "https://github.com/janesmith",
            FreeTextComment = "Skilled front-end developer."
        };

        // Act
        var result = _candidateMapper.EntityToModel(candidateEntity);

        // Assert
        Assert.Equal(candidateEntity.FirstName, result.FirstName);
        Assert.Equal(candidateEntity.LastName, result.LastName);
        Assert.Equal(candidateEntity.PhoneNumber, result.PhoneNumber);
        Assert.Equal(candidateEntity.Email, result.Email);
        Assert.Equal(candidateEntity.CallTimeInterval, result.CallTimeInterval);
        Assert.Equal(candidateEntity.LinkedInProfileUrl, result.LinkedInProfileUrl);
        Assert.Equal(candidateEntity.GitHubProfileUrl, result.GitHubProfileUrl);
        Assert.Equal(candidateEntity.FreeTextComment, result.FreeTextComment);
    }

    [Fact]
    public void ModelToEntity_ShouldMapNullPropertiesToEntity()
    {
        // Arrange
        var candidateModel = new CandidateModel
        {
            FirstName = "Alice",
            LastName = "Johnson",
            PhoneNumber = null,
            Email = "alicejohnson@example.com",
            CallTimeInterval = null,
            LinkedInProfileUrl = null,
            GitHubProfileUrl = null,
            FreeTextComment = null
        };

        // Act
        var result = _candidateMapper.ModelToEntity(candidateModel);

        // Assert
        Assert.Equal(candidateModel.FirstName, result.FirstName);
        Assert.Equal(candidateModel.LastName, result.LastName);
        Assert.Null(result.PhoneNumber);
        Assert.Equal(candidateModel.Email, result.Email);
        Assert.Null(result.CallTimeInterval);
        Assert.Null(result.LinkedInProfileUrl);
        Assert.Null(result.GitHubProfileUrl);
        Assert.Null(result.FreeTextComment);
    }

    [Fact]
    public void EntityToModel_ShouldMapNullPropertiesToModel()
    {
        // Arrange
        var candidateEntity = new Candidate
        {
            FirstName = "Bob",
            LastName = "Brown",
            PhoneNumber = null,
            Email = "bobbrown@example.com",
            CallTimeInterval = null,
            LinkedInProfileUrl = null,
            GitHubProfileUrl = null,
            FreeTextComment = null
        };

        // Act
        var result = _candidateMapper.EntityToModel(candidateEntity);

        // Assert
        Assert.Equal(candidateEntity.FirstName, result.FirstName);
        Assert.Equal(candidateEntity.LastName, result.LastName);
        Assert.Null(result.PhoneNumber);
        Assert.Equal(candidateEntity.Email, result.Email);
        Assert.Null(result.CallTimeInterval);
        Assert.Null(result.LinkedInProfileUrl);
        Assert.Null(result.GitHubProfileUrl);
        Assert.Null(result.FreeTextComment);
    }
}