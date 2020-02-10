using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using LanguageExt;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class CreateRoomHandler : CommandHandler<Room, Room.RoomId, IExecutionResult, CreateRoom>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            Room room,
            CreateRoom command,
            CancellationToken cancellationToken)
        {
            try
            {
                room.Create(command.Number);
                return ExecutionResult.Success().AsTask();
            }
            catch (RoomAlreadyOccupiedException e)
            {
                return ExecutionResult.Failed(e.Message).AsTask();
            }
        }
    }
}