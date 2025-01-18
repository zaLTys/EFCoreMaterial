using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseNpgsql(connectionString)
        .EnableSensitiveDataLogging() // Optional, logs parameter values
        .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<MyDbContext>();
        dbContext.Database.Migrate();

        ListMigrations(dbContext);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ListMigrations(MyDbContext myDbContext)
{
    var migrations = myDbContext.Database.GetAppliedMigrations();
    foreach (var migration in migrations)
    {
        Console.WriteLine($"Applied Migration: {migration}");
    }
}