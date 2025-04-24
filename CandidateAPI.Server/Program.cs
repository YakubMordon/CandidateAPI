using CandidateAPI.Application.Interfaces.Mappers;
using CandidateAPI.Application.Interfaces.Repositories;
using CandidateAPI.Application.Interfaces.Services;
using CandidateAPI.Infrastructure;
using CandidateAPI.Infrastructure.Mappers;
using CandidateAPI.Infrastructure.Repositories;
using CandidateAPI.Infrastructure.Services;
using CandidateAPI.ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<ApplicationDbContext>("CandidateDatabase");

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddScoped<ICandidateMapper, CandidateMapper>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
