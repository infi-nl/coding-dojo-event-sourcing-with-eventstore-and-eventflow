using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Sagas
{
    public class ReservationSagaLocator : ISagaLocator
    {
        public Task<ISagaId> LocateSagaAsync(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            if (domainEvent.IdentityType == typeof(ReservationId)) // ED Consider using a MetaDataProvider; overkill now.
            {
                var reservationId = domainEvent.GetIdentity().Value;

                var orderSagaId = ReservationSagaId.With(ReservationId.With(reservationId).GetGuid());

                return Task.FromResult<ISagaId>(orderSagaId);
            }

            return Task.FromResult<ISagaId>(ReservationSagaId.New);
        }
    }
}