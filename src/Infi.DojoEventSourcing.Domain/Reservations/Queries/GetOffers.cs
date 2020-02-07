using System;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    public class GetOffers : IQuery<ReservationOffer>
    {
        public GetOffers(ReservationId reservationId, DateTime arrival, DateTime departure)
        {
            ReservationId = reservationId;
            Arrival = arrival;
            Departure = departure;
        }

        public ReservationId ReservationId { get; }
        public DateTime Arrival { get; }
        public DateTime Departure { get; }
    }
}