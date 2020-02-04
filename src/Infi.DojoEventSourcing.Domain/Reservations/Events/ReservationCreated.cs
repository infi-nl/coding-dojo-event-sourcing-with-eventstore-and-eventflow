using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("ReservationCreated", 1)]
    public class ReservationCreated : AggregateEvent<Reservation, ReservationId>
    {
        public ReservationCreated(
            ReservationId id,
            in DateTime arrival,
            in DateTime departure,
            DateTime createCheckInTimeFromDate,
            DateTime createCheckOutTimeFromDate)
        {
            Id = id;
            Arrival = arrival;
            Departure = departure;
            CreateCheckInTimeFromDate = createCheckInTimeFromDate;
            CreateCheckOutTimeFromDate = createCheckOutTimeFromDate;
        }

        public ReservationId Id { get; }

        public DateTime Arrival { get; }

        public DateTime Departure { get; }

        public DateTime CreateCheckInTimeFromDate { get; }

        public DateTime CreateCheckOutTimeFromDate { get; }
    }
}