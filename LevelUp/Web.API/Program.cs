using DataAccessLayer;
using DataAccessLayer.ManualMigrations.VersionMetadata;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.EntityFrameworkCore;

namespace Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddConsole();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("Postgres");
            builder.Services.AddDbContext<MyDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                //.EnableSensitiveDataLogging() // Optional, logs parameter values
                //.LogTo(Console.WriteLine, LogLevel.Information);
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            //Fluent migrator
            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .WithGlobalCommandTimeout(TimeSpan.FromMinutes(15))
                    .ScanIn(typeof(MyDbContext).Assembly).For.Migrations())
                .AddScoped<IMigrationRunner, MigrationRunner>()
                .AddScoped<IVersionTableMetaData, CustomVersionTableMetaData>()
                .Configure<RunnerOptions>(opt =>
                {
                    opt.Tags = new[] { "Postgres" };
                });


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
            
            //Migration control
            using (var scope = app.Services.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                var configuration = app.Services.GetRequiredService<IConfiguration>();
                var migrateTo = configuration["ManualMigrations:MigrateTo"] ?? string.Empty;

                if (string.IsNullOrWhiteSpace(migrateTo))
                {
                    Console.WriteLine("No migration action taken.");
                    return;
                }

                if (migrateTo.Equals("latest", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Applying latest migrations...");
                    runner.MigrateUp();
                }
                else if (long.TryParse(migrateTo, out long targetVersion))
                {
                    Console.WriteLine($"Rolling back to migration version {targetVersion}...");
                    runner.RollbackToVersion(targetVersion);
                }
                else
                {
                    Console.WriteLine($"Invalid migration target: {migrateTo}. No action taken.");
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

            
        }
        
        private static void ListMigrations(MyDbContext myDbContext)
        {
            var migrations = myDbContext.Database.GetAppliedMigrations();
            foreach (var migration in migrations)
            {
                Console.WriteLine($"Applied Migration: {migration}");
            }
        }
    }
}