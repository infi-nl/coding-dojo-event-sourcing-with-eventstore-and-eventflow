using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Commands.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.CommandHandlers.Reservations
{
    public class PlaceReservationHandler
        : CommandHandler<Reservation, ReservationId, IExecutionResult, PlaceReservation>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            Reservation reservation,
            PlaceReservation command,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}