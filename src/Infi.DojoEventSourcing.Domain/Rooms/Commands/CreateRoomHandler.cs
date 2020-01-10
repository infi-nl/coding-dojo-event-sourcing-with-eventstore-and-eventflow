using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class CreateRoomHandler : CommandHandler<Room, Room.RoomIdentity, CreateRoom>
    {
        private readonly IRoomRepository _roomRepository;

        public CreateRoomHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public override Task ExecuteAsync(Room room, CreateRoom command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}