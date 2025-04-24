using Bogus;
using CandidateAPI.Domain.Entities;
using CandidateAPI.Domain.Models;

namespace CandidateAPI.Infrastructure.Tests.Fakers;

public class CandidateFaker : Faker<Candidate>
{
    public CandidateFaker()
    {
        RuleFor(c => c.FirstName, f => f.Name.FirstName());
        RuleFor(c => c.LastName, f => f.Name.LastName());
        RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+1-###-###-####"));
        RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName));
        RuleFor(c => c.CallTimeInterval, f => $"{f.Date.Soon().ToShortTimeString()} - {f.Date.Soon().AddHours(1).ToShortTimeString()}");
        RuleFor(c => c.LinkedInProfileUrl, f => $"https://www.linkedin.com/in/{f.Internet.UserName().ToLower()}");
        RuleFor(c => c.GitHubProfileUrl, f => $"https://github.com/{f.Internet.UserName().ToLower()}");
        RuleFor(c => c.FreeTextComment, f => f.Lorem.Paragraph());

        UseSeed(1994);
    }
}

public class CandidateModelFaker : Faker<CandidateModel>
{
    public CandidateModelFaker()
    {
        RuleFor(c => c.FirstName, f => f.Name.FirstName());
        RuleFor(c => c.LastName, f => f.Name.LastName());
        RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+1-###-###-####"));
        RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName));
        RuleFor(c => c.CallTimeInterval, f => $"{f.Date.Soon().ToShortTimeString()} - {f.Date.Soon().AddHours(1).ToShortTimeString()}");
        RuleFor(c => c.LinkedInProfileUrl, f => $"https://www.linkedin.com/in/{f.Internet.UserName().ToLower()}");
        RuleFor(c => c.GitHubProfileUrl, f => $"https://github.com/{f.Internet.UserName().ToLower()}");
        RuleFor(c => c.FreeTextComment, f => f.Lorem.Paragraph());

        UseSeed(1994);
    }
}