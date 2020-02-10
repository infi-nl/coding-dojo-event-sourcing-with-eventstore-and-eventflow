using System;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Repositories;
using LanguageExt;

namespace Infi.DojoEventSourcing.ReadModels.Api.DAL.Reservations
{
    public class OfferReadRepository : IOfferReadRepository
    {
        private readonly DbConnection _connection;

        public OfferReadRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ReservationOffer> GetAvailableOffersForReservation(
            ReservationId reservationId,
            DateTime arrival,
            DateTime departure)
        {
            var reservationOffers =
                await _connection
                    .QueryAsync<ReservationOffer>(
                        "SELECT * FROM Offer WHERE AggregateId = @ReservationId",
                        new { ReservationId = reservationId.Value });

            var offerLookup = reservationOffers.ToImmutableDictionary(_ => _.Date);

            // TODO ED Ported from the original example: consider just querying the database for relevant data; downside
            //         is that it moves BL outside of the responsible object (i.e. PriceOffered.IsStillValid)
            var totalPriceWithHack =
                Enumerable
                    .Range(0, departure.Subtract(arrival).Days)
                    .Select(offset => arrival.AddDays(offset))
                    .Aggregate(
                        Option<decimal>.Some(0.0m),
                        (maybePrice, date) =>
                        {
                            var a = offerLookup.ContainsKey(date);

                            var isStillValid = a && offerLookup[date].IsStillValid(DateTime.Now);

                            return (isStillValid)
                                ? maybePrice.Map(p => p + offerLookup[date].Price)
                                : Option<decimal>.None;
                        })
                    .IfNone(-1); // FIXME Hack ported from the original code

            return new ReservationOffer(reservationId.Value, arrival, departure, totalPriceWithHack);
        }
    }
}