using System;
using EventFlow.Aggregates;

namespace Infi.DojoEventSourcing.Domain.Rooms.Events
{
    public class RoomOccupied : IAggregateEvent<Room, Room.RoomIdentity>
    {
        public RoomOccupied(DateTime startDateUtc, DateTime endDateUtc, Guid occupant)
        {
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
            Occupant = occupant;
        }

        public DateTime StartDateUtc { get; }
        public DateTime EndDateUtc { get; }
        public Guid Occupant { get; }
    }
}