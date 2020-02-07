using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("RoomOccupyRequested", 1)]
    public class RoomOccupyRequested : AggregateEvent<Reservation, ReservationId>
    {
        public RoomOccupyRequested(Room.RoomIdentity roomId, DateTime arrival, DateTime departure)
        {
            RoomId = roomId;
            Arrival = arrival;
            Departure = departure;
        }

        public DateTime Departure { get; }

        public DateTime Arrival { get; }

        public Room.RoomIdentity RoomId { get; }
    }
}