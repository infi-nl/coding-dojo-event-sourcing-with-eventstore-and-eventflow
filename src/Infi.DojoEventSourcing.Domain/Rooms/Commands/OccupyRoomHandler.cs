using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyRoomHandler : CommandHandler<Room, Room.RoomIdentity, OccupyRoom>
    {
        public override Task ExecuteAsync(Room room, OccupyRoom command, CancellationToken cancellationToken)
        {
            room.Occupy(command.Range, Guid.NewGuid()); // FIXME ED Add occupant

            return Task.FromResult(0);
        }
    }
}