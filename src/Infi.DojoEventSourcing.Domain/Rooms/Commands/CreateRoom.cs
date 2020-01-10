using EventFlow.Commands;
using EventFlow.Core;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class CreateRoom : Command<Room, Room.RoomIdentity>
    {
        public CreateRoom(Room.RoomIdentity aggregateId) : base(aggregateId)
        {
        }

        public CreateRoom(Room.RoomIdentity aggregateId, ISourceId sourceId) : base(aggregateId, sourceId)
        {
        }
    }
}