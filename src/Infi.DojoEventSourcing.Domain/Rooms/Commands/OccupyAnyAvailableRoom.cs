using EventFlow.Commands;
using EventFlow.Core;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyAnyAvailableRoom : Command<Room, Room.RoomIdentity>
    {
        public OccupyAnyAvailableRoom(Room.RoomIdentity aggregateId) : base(aggregateId)
        {
        }

        public OccupyAnyAvailableRoom(Room.RoomIdentity aggregateId, ISourceId sourceId) : base(aggregateId, sourceId)
        {
        }
    }
}