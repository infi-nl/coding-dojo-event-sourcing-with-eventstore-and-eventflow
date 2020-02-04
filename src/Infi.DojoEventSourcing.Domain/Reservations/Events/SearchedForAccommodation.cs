using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("SearchedForAccommodation", 1)]
    public class SearchedForAccommodation : AggregateEvent<Reservation, ReservationId>
    {
        public SearchedForAccommodation(ReservationId id, in DateTime arrival, in DateTime departure)
        {
            Id = id;
            Arrival = arrival;
            Departure = departure;
        }

        public ReservationId Id { get; }

        public DateTime Arrival { get; }

        public DateTime Departure { get; }
    }
}