using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
{
    public class GetOffers : IQuery<ReservationOffer>
    {
        public GetOffers(string reservationId, DateTime arrival, DateTime departure)
        {
            ReservationId = reservationId;
            Arrival = arrival;
            Departure = departure;
        }

        public string ReservationId { get; }
        public DateTime Arrival { get; }
        public DateTime Departure { get; }
    }

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
            await _dbReadContext.RunAsync(factory => factory
                .CreateOffersRepository()
                .GetOfferById(query.ReservationId, query.Arrival, query.Departure));
    }
}