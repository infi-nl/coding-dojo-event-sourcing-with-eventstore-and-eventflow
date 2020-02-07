using EventFlow.Commands;
using EventFlow.Core;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyRoom : Command<Room, Room.RoomIdentity>
    {
        public OccupyRoom(Room.RoomIdentity aggregateId, Range range) : base(aggregateId)
        {
            Range = range;
        }

        public OccupyRoom(Room.RoomIdentity aggregateId, Range range, ISourceId sourceId) : base(aggregateId, sourceId)
        {
            Range = range;
        }

        public Range Range;
    }
}