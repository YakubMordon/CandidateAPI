using CandidateAPI.Domain.Models;

namespace CandidateAPI.Application.Interfaces.Services;

public interface ICandidateService : IApplicationService<CandidateModel>
{
    Task<CandidateModel?> GetByEmailAsync(string email);
}