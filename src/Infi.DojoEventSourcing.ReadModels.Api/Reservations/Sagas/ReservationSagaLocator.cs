using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Sagas
{
    public class OrderSagaLocator : ISagaLocator
    {
        public Task<ISagaId> LocateSagaAsync(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            var reservationId = domainEvent.Metadata["reservation-id"];
            var reservationSagaId = ReservationSagaId.With(reservationId);

            return Task.FromResult<ISagaId>(reservationSagaId);
        }
    }
}