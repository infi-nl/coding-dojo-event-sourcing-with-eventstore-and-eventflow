using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using static Infi.DojoEventSourcing.ReadModels.Api.Capacity.Queries.CapacityUtils;

namespace Infi.DojoEventSourcing.ReadModels.Api.Capacity.Queries
{
    public class GetCapacityByTimeRange : IQuery<CapacityDto[]>
    {
        public GetCapacityByTimeRange(DateTime arrival, DateTime departure)
        {
            Arrival = arrival;
            Departure = departure;
        }

        public DateTime Departure { get; }

        public DateTime Arrival { get; }
    }

    public class GetCapacityByTimeRangeHandler : IQueryHandler<GetCapacityByTimeRange, CapacityDto[]>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetCapacityByTimeRangeHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public Task<CapacityDto[]> ExecuteQueryAsync(
            GetCapacityByTimeRange query,
            CancellationToken cancellationToken) =>
            GetCapacityByDateRange(query.Arrival, query.Departure);

        private async Task<CapacityDto[]> GetCapacityByDateRange(DateTime arrival, DateTime departure)
        {
            var reservations =
                await _dbReadContext.RunAsync(f =>
                        f.CreateReservationRepository()
                            .GetByRange(arrival, departure))
                    .ConfigureAwait(false);


            // TODO ED Consider creating dedicated CapacityPerDate-RM
            var dateToReservationCountLookup = BuildReservationCountLookup(reservations);

            return BuildDatesFrom(arrival, departure)
                .Aggregate(
                    ImmutableList<CapacityDto>.Empty,
                    (capacities, date) =>
                        date <= departure
                            ? capacities.Add(MapToCapacity(date, dateToReservationCountLookup))
                            : capacities)
                .ToArray();
        }
    }
}