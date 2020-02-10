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
using Serilog;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class OccupyAnyAvailableRoomHandler
        : CommandHandler<Reservation, ReservationId, IExecutionResult, OccupyAnyAvailableRoom>
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
            try
            {
                var anyAvailableRoom = await GetAnyAvailableRoom(command.Start, command.End, cancellationToken);

                reservation.RequestOccupyRoom(anyAvailableRoom.RoomId, command.Start, command.End);

                return ExecutionResult.Success();
            }
            catch (Exception e)
            {
                Log.Error(e,
                    "Failed to occupy any available room for {reservationId}: {error}",
                    reservation.Id,
                    e.Message);
                return ExecutionResult.Failed(e.Message);
            }
        }

        private async Task<RoomAvailabilityDto> GetAnyAvailableRoom(
            DateTime start,
            DateTime end,
            CancellationToken cancellationToken)
        {
            var rooms =
                await _queryProcessor.ProcessAsync(new GetAvailabilityByDateRange(start, end), cancellationToken);

            rooms.Find(room => room.IsAvailable);
            if (!rooms.Any(room => room.IsAvailable))
            {
                throw new NoRoomsAvailableException($"No rooms available for {start} - {end}");
            }

            return rooms.First(room => room.IsAvailable);
        }
    }
}