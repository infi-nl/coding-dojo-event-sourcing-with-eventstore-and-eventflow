using System;
using EventFlow.Commands;
using EventFlow.Core;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class AssignRoom : Command<Room, Room.RoomIdentity>
    {
        public AssignRoom(Room.RoomIdentity aggregateId, DateTime start, DateTime end) : base(aggregateId)
        {
            Start = start;
            End = end;
        }

        public AssignRoom(Room.RoomIdentity aggregateId, DateTime start, DateTime end, ISourceId sourceId) :
            base(aggregateId, sourceId)
        {
            Start = start;
            End = end;
        }

        public DateTime End { get; }

        public DateTime Start { get; }
    }
}