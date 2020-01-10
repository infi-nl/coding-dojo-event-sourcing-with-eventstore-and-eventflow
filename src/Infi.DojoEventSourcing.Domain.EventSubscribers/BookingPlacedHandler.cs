using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.EventSubscribers
{
    public class BookingPlacedHandler
        : ISubscribeSynchronousTo<Booking, BookingId, BookingPlaced>
    {
        public Task HandleAsync(
            IDomainEvent<Booking, BookingId, BookingPlaced> domainEvent,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}