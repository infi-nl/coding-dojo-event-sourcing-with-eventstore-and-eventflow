using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyAnyAvailableRoomHandler : CommandHandler<Room, Room.RoomIdentity, OccupyAnyAvailableRoom>
    {
        private readonly IRoomRepository _roomRepository;

        public OccupyAnyAvailableRoomHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public override Task ExecuteAsync(Room room, OccupyAnyAvailableRoom command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}