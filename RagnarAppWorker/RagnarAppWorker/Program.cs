using Microsoft.EntityFrameworkCore;
using RagnarApp.Application.Abstract;
using RagnarApp.Infrastructure.Data;
using RagnarApp.Infrastructure.Repositories;
using RagnarAppWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<RagnarAppContext>(options =>
{
    options.UseNpgsql(
    builder.Configuration.GetConnectionString("Library"));
});

builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();

host.Run();