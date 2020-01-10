using EventFlow.Commands;
using EventFlow.Core;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyRoom : Command<Room, Room.RoomIdentity>
    {
        public OccupyRoom(Room.RoomIdentity aggregateId) : base(aggregateId)
        {
        }

        public OccupyRoom(Room.RoomIdentity aggregateId, ISourceId sourceId) : base(aggregateId, sourceId)
        {
        }
    }
}