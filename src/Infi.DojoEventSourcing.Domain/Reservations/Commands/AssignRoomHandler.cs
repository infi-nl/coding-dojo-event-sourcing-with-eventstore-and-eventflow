using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class AssignRoomHandler : CommandHandler<Room, Room.RoomIdentity, AssignRoom>
    {
        private readonly GetAvailabilityByTimeRangeHandler _getAvailabilityByTimeRangeHandler;

        public AssignRoomHandler(GetAvailabilityByTimeRangeHandler getAvailabilityByTimeRangeHandler)
        {
            _getAvailabilityByTimeRangeHandler = getAvailabilityByTimeRangeHandler;
        }

        public override async Task ExecuteAsync(Room room, AssignRoom command,
            CancellationToken cancellationToken)
        {
            var anyAvailableRoom =
                await GetAnyAvailableRoom(command.Start, command.End, cancellationToken).ConfigureAwait(false);

            // publisher.publish(new OccupyRoom(anyAvailableRoom., command.start, command.end, command.occupant));
        }

        private async Task<RoomAvailabilityDto> GetAnyAvailableRoom(DateTime start, DateTime end,
            CancellationToken cancellationToken)
        {
            var rooms =
                await _getAvailabilityByTimeRangeHandler
                    .ExecuteQueryAsync(new GetAvailabilityByTimeRange(start, end), cancellationToken)
                    .ConfigureAwait(false);

            var firstAvailableRoom = rooms.FirstOrDefault(room => room.IsAvailable);

            if (firstAvailableRoom == null)
            {
                throw new NoRoomsAvailableException();
            }

            return firstAvailableRoom;
        }
    }
}