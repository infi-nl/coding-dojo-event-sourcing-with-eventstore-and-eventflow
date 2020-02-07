using System;
using System.Globalization;
using System.Reflection;
using EventFlow.AspNetCore.Extensions;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EventStores.EventStore.Extensions;
using EventFlow.Extensions;
using EventFlow.SQLite.Connections;
using EventFlow.SQLite.Extensions;
using EventStore.ClientAPI;
using Infi.DojoEventSourcing.Configuration;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Pricings;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.Sagas;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;
using Infi.DojoEventSourcing.ReadModels.Api;
using Infi.DojoEventSourcing.ReadModels.Api.DAL;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DojoEventSourcing
{
    public class Startup
    {
        private static readonly IConfigurationRoot Configuration = ConfigurationFactory.Create();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var apiReadModelConnectionString = Configuration["ApiReadModel:ConnectionString"];

            services.AddControllers();

            services.AddSingleton(new OfferReadModelLocator()); // FIXME ED Used this to fix a DI issue

            services.AddEventFlow(
                cfg =>
                {
                    cfg
                        .Configure(c => c.ThrowSubscriberExceptions = true)
                        .AddAspNetCore(
                            o => o
                                .RunBootstrapperOnHostStartup()
                                .AddUriMetadata())
                        .UseEventStoreEventStore(
                            new Uri(Configuration["EventStore:ConnectionString"]),
                            ConnectionSettings.Create()
                                .KeepReconnecting()
                                .LimitReconnectionsTo(
                                    int.Parse(
                                        Configuration["EventStore:LimitReconnectionsTo"],
                                        CultureInfo.InvariantCulture))
                                .SetReconnectionDelayTo(
                                    TimeSpan.FromSeconds(
                                        int.Parse(
                                            Configuration["EventStore:ReconnectionDelayInSeconds"],
                                            CultureInfo.InvariantCulture))),
                            Assembly.GetExecutingAssembly().GetName().Name)
                        .ConfigureSQLite(
                            SQLiteConfiguration.New.SetConnectionString(apiReadModelConnectionString))
                        .AddCommandHandlers(typeof(MakeReservationHandler).Assembly)
                        .AddEvents(typeof(ReservationCreated).Assembly)
                        .UseSQLiteReadModel<RoomReadModel>()
                        .UseSQLiteReadModel<ReservationReadModel>()
                        .UseSQLiteReadModel<OfferReadModel, OfferReadModelLocator>()
                        .UseSQLiteReadModel<RoomOccupationReadModel>()
                        .AddQueryHandlers(typeof(GetAllReservationsHandler).Assembly)
                        .AddSagaLocators(typeof(ReservationSagaLocator))
                        .AddSagas(typeof(ReservationSaga))
                        .UseLibLog(LibLogProviders.Serilog);
                });

            var databaseReadContext = ApiReadContextFactory.Create(apiReadModelConnectionString);
            services.AddScoped<IDatabaseContext<IApiReadModelRepositoryFactory>>(_ => databaseReadContext);
            services.AddScoped<IPricingEngine, RandomPricingEngine>(); // FIXME ED Use InMemoryPricingEngine
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}