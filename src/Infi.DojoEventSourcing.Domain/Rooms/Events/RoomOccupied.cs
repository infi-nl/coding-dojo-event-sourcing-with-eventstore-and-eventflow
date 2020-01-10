using System;
using EventFlow.Aggregates;

namespace Infi.DojoEventSourcing.Domain.Rooms.Events
{
    public class RoomOccupied : IAggregateEvent, IAggregateEvent<Room, Room.RoomIdentity>
    {
        public RoomOccupied(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public RoomOccupied(Room.RoomIdentity start, in DateTime rangeStart, in DateTime rangeEnd, Guid occupant)
        {
            throw new NotImplementedException();
        }

        public DateTime End { get; }
        public DateTime Start { get; }
    }
}