using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyAnyAvailableRoomHandler : CommandHandler<Reservation, ReservationId, OccupyAnyAvailableRoom>
    {
        private readonly IQueryProcessor _queryProcessor;

        public OccupyAnyAvailableRoomHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task ExecuteAsync(
            Reservation reservation,
            OccupyAnyAvailableRoom command,
            CancellationToken cancellationToken)
        {
            var anyAvailableRoom =
                await GetAnyAvailableRoom(command.Start, command.End, cancellationToken).ConfigureAwait(false);

            reservation.RequestOccupyRoom(anyAvailableRoom.RoomId, command.Start, command.End);
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

            if (rooms.Length() == 0)
            {
                throw new NoRoomsAvailableException();
            }

            return rooms.First(room => room.IsAvailable);
        }
    }
}