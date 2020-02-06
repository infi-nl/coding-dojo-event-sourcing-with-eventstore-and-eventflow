using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
{
    public class GetCapacityByDate : IQuery<CapacityDto>
    {
        public GetCapacityByDate(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
    }

    public class GetCapacityByDateHandler : IQueryHandler<GetCapacityByDate, CapacityDto>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetCapacityByDateHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public Task<CapacityDto> ExecuteQueryAsync(
            GetCapacityByDate query,
            CancellationToken cancellationToken) =>
            GetCapacityByDate(query.Date);

        private async Task<CapacityDto> GetCapacityByDate(DateTime date)
        {
            var reservations =
                await _dbReadContext
                    .RunAsync(f => f.CreateReservationRepository().GetByRange(date, date))
                    .ConfigureAwait(false);

            var dateToReservationCountLookup =
                reservations
                    .GroupBy(_ => _.Arrival)
                    .ToImmutableDictionary(_ => _.Key, _ => _.Count());

            return new CapacityDto
            {
                Date = date,
                Capacity = 42, // FIXME ED Read actual capacity
                Reserved = dateToReservationCountLookup.GetValueOrDefault(date)
            };
        }
    }
}