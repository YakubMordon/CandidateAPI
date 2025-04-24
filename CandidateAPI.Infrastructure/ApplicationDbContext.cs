using CandidateAPI.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150) 
                .IsUnicode(false); 

            entity.Property(e => e.FirstName)
                .IsRequired() 
                .HasMaxLength(100); 

            entity.Property(e => e.LastName)
                .IsRequired() 
                .HasMaxLength(100);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(e => e.CallTimeInterval)
                .HasMaxLength(100); 

            entity.Property(e => e.LinkedInProfileUrl)
                .HasMaxLength(200) 
                .IsUnicode(false);  

            entity.Property(e => e.GitHubProfileUrl)
                .HasMaxLength(200) 
                .IsUnicode(false);  

            entity.Property(e => e.FreeTextComment)
                .IsRequired() 
                .HasMaxLength(1000); 
        });
    }
}