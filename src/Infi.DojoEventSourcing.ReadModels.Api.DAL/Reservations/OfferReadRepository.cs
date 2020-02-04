using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;

namespace Infi.DojoEventSourcing.ReadModels.Api.DAL.Reservations
{
    public class OfferReadRepository : IOfferReadRepository
    {
        private readonly DbConnection _connection;

        public OfferReadRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ReservationOffer> GetOfferById(
            string reservationId,
            DateTime arrival,
            DateTime departure)
        {
            // FIXME ED Filter on query parameters
            var reservationOffer =
                await _connection
                    .QueryAsync<ReservationOffer>(
                        @"SELECT * FROM Offer");

            return reservationOffer.First(); // FIXME ED Error handling
        }
    }
}