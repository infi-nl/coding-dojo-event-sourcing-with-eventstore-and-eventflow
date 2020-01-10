using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyRoomHandler : CommandHandler<Room, Room.RoomIdentity, OccupyRoom>
    {
        private readonly IRoomRepository _roomRepository;

        public OccupyRoomHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public override Task ExecuteAsync(Room room, OccupyRoom command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}