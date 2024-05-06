using Ordering.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.Run();
