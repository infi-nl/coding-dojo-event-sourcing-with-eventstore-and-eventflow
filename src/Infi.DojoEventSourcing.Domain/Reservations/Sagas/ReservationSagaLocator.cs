using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.Sagas;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms.Events;

namespace Infi.DojoEventSourcing.Domain.Reservations.Sagas
{
    public class ReservationSagaLocator : ISagaLocator
    {
        public Task<ISagaId> LocateSagaAsync(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            var identity = GetIdentityTypeForEvent(domainEvent);
            if (identity.GetType() == typeof(ReservationId)) // ED Consider using a MetaDataProvider; overkill now.
            {
                var reservationId = identity.Value;

                var orderSagaId = ReservationSagaId.With(ReservationId.With(reservationId).GetGuid());

                return Task.FromResult<ISagaId>(orderSagaId);
            }


            return Task.FromResult<ISagaId>(ReservationSagaId.New);
        }

        private IIdentity GetIdentityTypeForEvent(IDomainEvent @event)
        {
            if (@event.GetAggregateEvent() is RoomOccupied roomOccupied)
            {
                return roomOccupied.ReservationId;
            }

            return @event.GetIdentity();
        }
    }
}