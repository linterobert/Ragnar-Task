using Microsoft.EntityFrameworkCore;
using RagnarApp.Application.Abstract;
using RagnarApp.Application.Queries.LibraryQueries;
using RagnarApp.Infrastructure.Data;
using RagnarApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<RagnarAppContext>(options =>
{
    options.UseNpgsql(
    builder.Configuration.GetConnectionString("Library"));
});

builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddSingleton<IMessagePublisher>(_ => new RabbitMqPublisher(
    builder.Configuration["RabbitMq:HostName"] ?? "localhost",
    builder.Configuration["RabbitMq:UserName"] ?? "guest",
    builder.Configuration["RabbitMq:Password"] ?? "guest"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddMediatR(cfg =>
    {
    cfg.RegisterServicesFromAssembly(typeof(GetLibrariesQuery).Assembly);
    });
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RagnarAppContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok());
app.MapControllers();

app.Run();
