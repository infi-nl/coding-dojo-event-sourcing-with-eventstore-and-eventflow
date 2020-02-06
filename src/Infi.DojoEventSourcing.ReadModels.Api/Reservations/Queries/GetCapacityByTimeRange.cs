using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
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

            var dateToReservationCountLookup =
                reservations
                    .GroupBy(_ => _.Arrival)
                    .ToImmutableDictionary(_ => _.Key, _ => _.Count());

            return Enumerable
                .Range(0, departure.Subtract(arrival).Days)
                .Select(offset => arrival.AddDays(offset))
                .Aggregate(
                    ImmutableList<CapacityDto>.Empty,
                    (capacities, date) =>
                        date <= departure
                            ? capacities.Add(MapToCapacity(date, dateToReservationCountLookup))
                            : capacities)
                .ToArray();
        }

        private static CapacityDto MapToCapacity(DateTime date, IReadOnlyDictionary<DateTime, int> reservation) =>
            new CapacityDto
            {
                Date = date,
                Capacity = 42, // FIXME ED Read actual capacity
                Reserved = reservation.GetValueOrDefault(date)
            };
    }
}