using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class CreateRoomHandler : CommandHandler<Room, Room.RoomIdentity, CreateRoom>
    {
        public override Task ExecuteAsync(Room room, CreateRoom command, CancellationToken cancellationToken)
        {
            room.Create(command.Number);

            return Task.FromResult(0); // FIXME ED Magic number
        }
    }
}