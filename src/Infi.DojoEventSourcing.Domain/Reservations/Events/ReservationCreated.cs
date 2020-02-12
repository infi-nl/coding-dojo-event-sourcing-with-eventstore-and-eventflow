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
            DateTime arrival,
            DateTime departure,
            DateTime checkInTime,
            DateTime checkOutTime)
        {
            Id = id;
            Arrival = arrival;
            Departure = departure;
            CheckInTime = checkInTime;
            CheckOutTime = checkOutTime;
        }

        public ReservationId Id { get; }

        public DateTime Arrival { get; }

        public DateTime Departure { get; }

        public DateTime CheckInTime { get; }

        public DateTime CheckOutTime { get; }
    }
}