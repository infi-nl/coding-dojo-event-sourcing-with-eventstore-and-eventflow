using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using LanguageExt;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class AssignRoomHandler : CommandHandler<Reservation, ReservationId, IExecutionResult, AssignRoom>
    {
        public override async Task<IExecutionResult> ExecuteCommandAsync(
            Reservation reservation,
            AssignRoom command,
            CancellationToken cancellationToken)
        {
            try
            {
                // FIXME Should check if room exists
                reservation.AssignRoom(command.RoomId);
                return await ExecutionResult.Success().AsTask();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await ExecutionResult.Failed(e.Message).AsTask();
            }
        }
    }
}