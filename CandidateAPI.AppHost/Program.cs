using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("mssql")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("CandidateDatabase");

builder.AddProject<Projects.CandidateAPI_Server>("web-server")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
