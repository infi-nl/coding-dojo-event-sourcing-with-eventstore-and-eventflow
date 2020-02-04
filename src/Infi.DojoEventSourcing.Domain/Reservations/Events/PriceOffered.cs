using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("PriceOffered", 1)]
    public class PriceOffered : AggregateEvent<Reservation, ReservationId>
    {
        public PriceOffered(ReservationId reservationId, DateTime date, Money price, DateTime expires)
        {
            AggregateId = Guid.NewGuid().ToString();
            ReservationId = reservationId;
            Date = date;
            Price = price;
            Expires = expires;
        }

        public string AggregateId { get; }
        public ReservationId ReservationId { get; }
        public DateTime Date { get; }
        public Money Price { get; }
        public DateTime Expires { get; }

        public bool IsInRange(DateTime arrival, DateTime departure) =>
            (Date.Equals(arrival) || Date > arrival)
            && Date < departure;

        public bool HasExpired() => !IsStillValid();

        public bool IsStillValid() => Expires > DateTime.UtcNow;
    }
}