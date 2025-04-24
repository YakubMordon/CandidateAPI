using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Infrastructure.Repositories;

public class CandidateRepository(ApplicationDbContext context) : BaseRepository<Candidate>(context), ICandidateRepository
{
    public override async Task DeleteAsync(Candidate entity)
    {
        var candidate = context.Candidates.FirstOrDefault(candidate => candidate.Email.Equals(entity.Email));
        if (candidate is not null)
        {
            await base.DeleteAsync(entity);
        }
    }

    public async Task<Candidate?> GetByEmailAsync(string email) =>
        await context.Set<Candidate>().FirstOrDefaultAsync(entity => entity.Email.Equals(email));
}