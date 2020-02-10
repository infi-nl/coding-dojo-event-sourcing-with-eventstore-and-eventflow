using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using LanguageExt;

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
                room.Occupy(command.ReservationId, command.Range);
                return ExecutionResult.Success().AsTask();
            }
            catch (RoomAlreadyOccupiedException e)
            {
                return ExecutionResult.Failed(e.Message).AsTask();
            }
        }
    }
}