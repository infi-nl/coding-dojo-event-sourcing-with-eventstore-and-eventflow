using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Rooms.Events
{
    [EventVersion("RoomOccupied", 1)]
    public class RoomOccupied : IAggregateEvent<Room, Room.RoomId>
    {
        public RoomOccupied(ReservationId reservationId, DateTime startDateUtc, DateTime endDateUtc)
        {
            ReservationId = reservationId;
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
        }

        public DateTime StartDateUtc { get; }
        public DateTime EndDateUtc { get; }
        public ReservationId ReservationId { get; }
    }
}