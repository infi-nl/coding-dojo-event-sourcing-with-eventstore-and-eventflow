using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using LanguageExt;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class MakeReservationHandler : CommandHandler<Reservation, ReservationId, IExecutionResult, MakeReservation>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            Reservation reservation,
            MakeReservation command,
            CancellationToken cancellationToken)
        {
            try
            {
                reservation.UpdateContactInformation(command.Name, command.Email);
                reservation.MakeReservation(command.Arrival, command.Departure);
                return ExecutionResult.Success().AsTask();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ExecutionResult.Failed().AsTask();
            }
        }
    }
}