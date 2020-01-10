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
using Infi.DojoEventSourcing.Domain.CommandHandlers.Reservations;
using Infi.DojoEventSourcing.Domain.EventSubscribers.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
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
            services.AddControllers();
            services.AddEventFlow(
                cfg =>
                {
                    cfg
                        .AddAspNetCore(
                            o => o
                                .RunBootstrapperOnHostStartup()
                                .AddUriMetadata())
                        .UseEventStoreEventStore(
                            new Uri(Configuration["EventStore:Uri"]),
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
                            SQLiteConfiguration.New.SetConnectionString(Configuration["SqlLite:ConnectionString"]))
                        .AddEvents(typeof(ReservationPlaced).Assembly)
                        .AddCommandHandlers(typeof(PlaceReservationHandler).Assembly)
                        .AddSubscribers(typeof(ReservationPlacedHandler))
                        .UseSQLiteReadModel<ReservationReadModel>()
                        .UseLibLog(LibLogProviders.Serilog);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}