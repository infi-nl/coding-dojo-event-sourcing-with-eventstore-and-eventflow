using EventFlow.Commands;
using EventFlow.Core;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class CreateRoom : Command<Room, Room.RoomId>
    {
        public CreateRoom(Room.RoomId aggregateId, string number)
            : base(aggregateId)
        {
            Number = number;
        }

        public CreateRoom(Room.RoomId aggregateId, string number, ISourceId sourceId)
            : base(aggregateId, sourceId)
        {
            Number = number;
        }

        public string Number { get; }
    }
}