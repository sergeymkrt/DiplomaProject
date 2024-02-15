using DiplomaProject.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiplomaProject.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the DbContext using the <paramref name="connectionString"/> connection string
    /// while setting up resilient SQL connections.
    /// </summary>
    /// <typeparam name="TContext">The context we are setting up which inherits <see cref="DbContext"/></typeparam>
    /// <param name="services">The services collection injected</param>
    /// <param name="connectionString">The connection string read from configuration</param>
    /// <param name="enableLogging">
    /// Whether to allow logging. Should be enabled in Development only to avoid incurring performance issues.
    /// </param>
    /// <returns>The services collection container to supported chaining.</returns>
    public static IServiceCollection AddSqlServerDbContext<TContext>(
        this IServiceCollection services,
        string connectionString, bool enableLogging = false) where TContext : DbContext
    {
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString), @"Connection string cannot be empty");

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(1500), null);
                    sqlOptions.MigrationsAssembly(typeof(WritableDbContext).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            if (enableLogging)
            {
                options.EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
        });

        return services;
    }

    public static async Task<IApplicationBuilder> UseDatabaseMigration(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<WritableDbContext>();
        await context?.Database?.MigrateAsync();

        return app;
    }

    public static async Task<IApplicationBuilder> UseInitialDataSeeding(this IApplicationBuilder app)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            return app;
        }

        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbInitializer = serviceScope.ServiceProvider.GetService<IDbInitializer>();

        await dbInitializer.SeedAsync();

        return app;
    }
}