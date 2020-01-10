using System;
using System.Reflection;
using FluentMigrator.Runner;
using Infi.DojoEventSourcing.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator
{
    public static class Program
    {
        private static readonly IConfiguration Configuration = ConfigurationFactory.Create();

        public static void Main()
        {
            Log.Information("Started Infi.DojoEventSourcing.ReadModelDbMigrator");
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        private static void UpdateDatabase(IServiceProvider scopeServiceProvider)
        {
            scopeServiceProvider
                .GetRequiredService<IMigrationRunner>()
                .MigrateUp();
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(
                    rb => rb
                        .AddSQLite()
                        .WithGlobalConnectionString(Configuration["SqlLite:ConnectionString"])
                        .ScanIn(Assembly.GetExecutingAssembly())
                        .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}