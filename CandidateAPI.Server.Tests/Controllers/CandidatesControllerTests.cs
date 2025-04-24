using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Domain.Models;
using CandidateAPI.Server.Controllers;
using CandidateAPI.Server.Tests.Fakers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CandidateAPI.Server.Tests.Controllers;

public class CandidatesControllerTests
{
    private readonly CandidateModelFaker _faker;
    private readonly Mock<ICandidateService> _mockCandidateService;
    private readonly CandidatesController _controller;

    public CandidatesControllerTests()
    {
        _faker = new CandidateModelFaker();
        _mockCandidateService = new Mock<ICandidateService>();
        _controller = new CandidatesController(_mockCandidateService.Object);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ModelStateInvalid_ReturnsBadRequest()
    {
        // Arrange
        var model = new CandidateModel
        {
            Email = "invalidEmail",
            FirstName = "John",
            LastName = "Doe"
        };
        _controller.ModelState.AddModelError("Email", "Invalid email format");

        // Act
        var result = await _controller.AddOrUpdateCandidate(model);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task AddOrUpdateCandidate_CandidateDoesNotExist_CallsAddAsync()
    {
        // Arrange
        var model = _faker.Generate();
        _mockCandidateService.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((CandidateModel)null); // No existing candidate

        // Act
        var result = await _controller.AddOrUpdateCandidate(model);

        // Assert
        _mockCandidateService.Verify(x => x.AddAsync(It.IsAny<CandidateModel>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task AddOrUpdateCandidate_CandidateExists_CallsUpdateAsync()
    {
        // Arrange
        var model = _faker.Generate();
        _mockCandidateService.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(model);

        // Act
        var result = await _controller.AddOrUpdateCandidate(model);

        // Assert
        _mockCandidateService.Verify(x => x.UpdateAsync(It.IsAny<CandidateModel>()), Times.Once);
        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task AddOrUpdateCandidate_ExceptionOccurs_ReturnsInternalServerError()
    {
        // Arrange
        var model = _faker.Generate();

        _mockCandidateService.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.AddOrUpdateCandidate(model);

        // Assert
        var statusCodeResult = result as ObjectResult;
        statusCodeResult.Should().NotBeNull(); 
        statusCodeResult.StatusCode.Should().Be(500); 
        statusCodeResult.Value.Should().Be("An error occurred while saving the candidate: Database error"); 
    }
}