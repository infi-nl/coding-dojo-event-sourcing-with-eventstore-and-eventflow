using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class OccupyAnyAvailableRoomHandler : CommandHandler<Reservation, ReservationId, IExecutionResult, OccupyAnyAvailableRoom>
    {
        private readonly IQueryProcessor _queryProcessor;

        public OccupyAnyAvailableRoomHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public override async Task<IExecutionResult> ExecuteCommandAsync(
            Reservation reservation,
            OccupyAnyAvailableRoom command,
            CancellationToken cancellationToken)
        {
            var anyAvailableRoom =
                await GetAnyAvailableRoom(command.Start, command.End, cancellationToken)
                    .ConfigureAwait(false);

            reservation.RequestOccupyRoom(anyAvailableRoom.RoomId, command.Start, command.End);

            return new SuccessExecutionResult();
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

            if (!rooms.Any(room => room.IsAvailable))
            {
                throw new NoRoomsAvailableException();
            }

            return rooms.First(room => room.IsAvailable);
        }
    }
}