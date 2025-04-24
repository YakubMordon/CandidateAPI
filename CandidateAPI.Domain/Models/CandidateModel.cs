using System.ComponentModel.DataAnnotations;

namespace CandidateAPI.Domain.Models;

public class CandidateModel : BaseModel
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [MaxLength(100)]
    public string CallTimeInterval { get; set; }

    [Url]
    [MaxLength(200)]
    public string LinkedInProfileUrl { get; set; }

    [Url]
    [MaxLength(200)]
    public string GitHubProfileUrl { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string FreeTextComment { get; set; }
}