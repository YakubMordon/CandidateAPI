using CandidateAPI.Domain.Entities;

namespace CandidateAPI.Application.Interfaces.Repositories;

public interface ICandidateRepository : IRepository<Candidate>
{
    Task<Candidate?> GetByEmailAsync(string email);
}