using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.EventSubscribers.Reservations
{
    public class ReservationPlacedHandler
        : ISubscribeSynchronousTo<Reservation, ReservationId, ReservationPlaced>
    {
        public Task HandleAsync(
            IDomainEvent<Reservation, ReservationId, ReservationPlaced> domainEvent,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}