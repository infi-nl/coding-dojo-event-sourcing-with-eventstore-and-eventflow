using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using static Infi.DojoEventSourcing.ReadModels.Api.Capacity.Queries.CapacityUtils;

namespace Infi.DojoEventSourcing.ReadModels.Api.Capacity.Queries
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
                    .RunAsync(f => f.CreateReservationRepository().GetByDate(date))
                    .ConfigureAwait(false);

            var dateToReservationCountLookup = BuildReservationCountLookup(reservations);

            return MapToCapacity(date, dateToReservationCountLookup);
        }
    }
}