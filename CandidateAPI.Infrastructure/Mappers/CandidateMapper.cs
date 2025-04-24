using CandidateAPI.Application.Interfaces.Mappers;
using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;

namespace CandidateAPI.Infrastructure.Mappers;

public class CandidateMapper : ICandidateMapper
{
    public Candidate ModelToEntity(CandidateModel model) =>
        new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            CallTimeInterval = model.CallTimeInterval,
            LinkedInProfileUrl = model.LinkedInProfileUrl,
            GitHubProfileUrl = model.GitHubProfileUrl,
            FreeTextComment = model.FreeTextComment,
        };

    public CandidateModel EntityToModel(Candidate entity) =>
        new()
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            CallTimeInterval = entity.CallTimeInterval,
            LinkedInProfileUrl = entity.LinkedInProfileUrl,
            GitHubProfileUrl = entity.GitHubProfileUrl,
            FreeTextComment = entity.FreeTextComment,
        };
}