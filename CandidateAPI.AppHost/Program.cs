using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("mssql")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("CandidateDatabase");

var redis = builder.AddRedis("redis");

builder.AddProject<Projects.CandidateAPI_Server>("web-server")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
