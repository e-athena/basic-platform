// Run an Athena application.
WebApplication
    .CreateBuilder(args)
    .AddAthena()
    .Build()
    .RunAthena<Program>();