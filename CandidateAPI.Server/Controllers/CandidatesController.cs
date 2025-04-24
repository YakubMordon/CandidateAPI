using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;
using CandidateAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController(ICandidateService candidateService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CandidateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        try
        {
            var existingCandidate = await candidateService.GetByEmailAsync(model.Email);

            if (existingCandidate is not null)
            {
                await candidateService.UpdateAsync(model);
            }
            else
            {
                await candidateService.AddAsync(model);
            }

            return Ok("Candidate profile saved successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while saving the candidate: {ex.Message}");
        }
    }
}