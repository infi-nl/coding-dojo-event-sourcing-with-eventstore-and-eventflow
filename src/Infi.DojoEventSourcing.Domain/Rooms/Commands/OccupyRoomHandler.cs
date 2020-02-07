using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyRoomHandler : CommandHandler<Room, Room.RoomId, IExecutionResult, OccupyRoom>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            Room room,
            OccupyRoom command,
            CancellationToken cancellationToken)
        {
            try
            {
                room.Occupy(command.ReservationId, command.Range, Guid.NewGuid()); // FIXME ED Add occupant
                return Task.FromResult((IExecutionResult)new SuccessExecutionResult());
            }
            catch (RoomAlreadyOccupiedException e)
            {
                return Task.FromResult((IExecutionResult)new FailedExecutionResult(new[] { e.Message }));
            }
        }
    }
}