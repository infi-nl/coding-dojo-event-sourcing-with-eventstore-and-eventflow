using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
{
    public class GetOffersHandler : IQueryHandler<GetOffers, ReservationOffer>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetOffersHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<ReservationOffer> ExecuteQueryAsync(
            GetOffers query,
            CancellationToken cancellationToken) =>
            await _dbReadContext
                .RunAsync(factory => factory
                    .CreateOffersRepository()
                    .GetAvailableOffersForReservation(query.ReservationId, query.Arrival, query.Departure));
    }
}