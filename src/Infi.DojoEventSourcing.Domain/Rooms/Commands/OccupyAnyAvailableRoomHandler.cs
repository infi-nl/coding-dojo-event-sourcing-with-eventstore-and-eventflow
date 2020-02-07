using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyAnyAvailableRoomHandler : CommandHandler<Room, Room.RoomIdentity, OccupyAnyAvailableRoom>
    {
        private readonly IQueryProcessor _queryProcessor;

        public OccupyAnyAvailableRoomHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task ExecuteAsync(
            Room room,
            OccupyAnyAvailableRoom command,
            CancellationToken cancellationToken)
        {
            var anyAvailableRoom =
                await GetAnyAvailableRoom(command.Start, command.End, cancellationToken).ConfigureAwait(false);

            // FIXME ED Occupy room.
            // room.Occupy();
        }

        private async Task<RoomAvailabilityDto> GetAnyAvailableRoom(
            DateTime start,
            DateTime end,
            CancellationToken cancellationToken)
        {
            var rooms =
                await _queryProcessor
                    .ProcessAsync(new GetAvailabilityByTimeRange(start, end), cancellationToken)
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