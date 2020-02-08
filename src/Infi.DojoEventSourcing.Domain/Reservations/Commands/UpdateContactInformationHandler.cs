using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using LanguageExt;
using Serilog;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class UpdateContactInformationHandler
        : CommandHandler<Reservation, ReservationId, IExecutionResult, UpdateContactInformation>
    {
        public override async Task<IExecutionResult> ExecuteCommandAsync(
            Reservation reservation,
            UpdateContactInformation command,
            CancellationToken cancellationToken)
        {
            try
            {
                reservation.UpdateContactInformation(command.Name, command.Email);
                return await ExecutionResult.Success().AsTask();
            }
            catch (Exception e)
            {
                Log.Error(e,
                    "Failed to update contact information for {reservationId}: {error}",
                    reservation.Id,
                    e.Message);
                return await ExecutionResult.Failed(e.Message).AsTask();
            }
        }
    }
}