using System;
using EventFlow.Commands;
using EventFlow.Core;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyAnyAvailableRoom : Command<Room, Room.RoomIdentity>
    {
        public OccupyAnyAvailableRoom(Room.RoomIdentity aggregateId, DateTime start, DateTime end) : base(aggregateId)
        {
            Start = start;
            End = end;
        }

        public OccupyAnyAvailableRoom(Room.RoomIdentity aggregateId, DateTime start, DateTime end, ISourceId sourceId) :
            base(aggregateId, sourceId)
        {
            Start = start;
            End = end;
        }

        public DateTime End { get; }

        public DateTime Start { get; }
    }
}