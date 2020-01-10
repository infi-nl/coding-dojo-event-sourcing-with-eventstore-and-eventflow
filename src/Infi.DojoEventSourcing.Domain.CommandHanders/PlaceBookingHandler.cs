using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Commands;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.CommandHandlers
{
    public class PlaceBookingHandler : CommandHandler<Booking, BookingId, IExecutionResult, PlaceBooking>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            Booking booking,
            PlaceBooking command,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}