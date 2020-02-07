using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class CreateRoomHandler : CommandHandler<Room, Room.RoomId, IExecutionResult, CreateRoom>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            Room room,
            CreateRoom command,
            CancellationToken cancellationToken)
        {
            room.Create(command.Number);

            return Task.FromResult((IExecutionResult)new SuccessExecutionResult());
        }
    }
}