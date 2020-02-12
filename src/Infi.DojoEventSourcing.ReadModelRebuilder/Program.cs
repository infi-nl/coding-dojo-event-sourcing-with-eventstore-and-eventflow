using System;
using System.Threading;
using System.Threading.Tasks;
using DojoEventSourcing;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EventStores.EventStore.Extensions;
using EventFlow.Extensions;
using EventFlow.SQLite.Connections;
using EventFlow.SQLite.Extensions;
using EventStore.ClientAPI;
using Infi.DojoEventSourcing.Configuration;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ReadModelRebuilder
{
    class Program
    {
        private static readonly IConfigurationRoot Configuration = ConfigurationFactory.Create();

        public static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            var serviceProvider = SetupDependencyInjection();
            var readModelRebuilder = serviceProvider.GetService<ReadmodelRebuildService>();

            await readModelRebuilder.RebuildAllReadModel<ReservationReadModel>(CancellationToken.None);
        }

        private static IServiceProvider SetupDependencyInjection()
        {
            var services = new ServiceCollection();

            services.AddEventFlow(o => o
                .UseEventStoreEventStore(
                    new Uri(Configuration["EventStore:ConnectionString"]),
                    ConnectionSettings.Default)
                .AddEvents(typeof(ReservationCreated).Assembly)
                .ConfigureSQLite(
                    SQLiteConfiguration.New.SetConnectionString(Configuration["ApiReadModel:ConnectionString"]))
                .UseSQLiteReadModel<RoomReadModel>()
                .UseSQLiteReadModel<ReservationReadModel>()
                .UseSQLiteReadModel<OfferReadModel, OfferReadModelLocator>()
                .UseSQLiteReadModel<RoomOccupationReadModel, RoomOccupationReadModelLocator>()
                .UseLibLog(LibLogProviders.Serilog));

            services.AddSingleton(new OfferReadModelLocator()); // FIXME ED Used this to fix a DI issue
            services.AddSingleton(new RoomOccupationReadModelLocator()); // FIXME ED Used this to fix a DI issue

            services.AddScoped<ReadmodelRebuildService>();

            return services.BuildServiceProvider();
        }
    }
}