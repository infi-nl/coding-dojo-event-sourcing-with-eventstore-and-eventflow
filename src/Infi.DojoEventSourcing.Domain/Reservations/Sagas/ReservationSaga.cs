using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Commands;
using Infi.DojoEventSourcing.Domain.Rooms.Events;

namespace Infi.DojoEventSourcing.Domain.Reservations.Sagas
{
    public class ReservationSaga
        : AggregateSaga<ReservationSaga, ReservationSagaId, ReservationSagaLocator>,
            ISagaIsStartedBy<Reservation, ReservationId, ReservationCreated>,
            IApply<RoomOccupied>,
            IApply<RoomAssigned>

    {
        private ReservationId _reservationId;

        public ReservationSaga(ReservationSagaId id) : base(id)
        {
        }

        public Task HandleAsync(IDomainEvent<Reservation, ReservationId, ReservationCreated> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Publish(new OccupyAnyAvailableRoom(
                Room.RoomIdentity.New, // TODO ED Reconsider this: only providing it, b/c EventFlow forces me to
                domainEvent.AggregateEvent.CreateCheckInTimeFromDate,
                domainEvent.AggregateEvent.CreateCheckOutTimeFromDate));

            _reservationId = domainEvent.AggregateEvent.Id; // TODO ED Any other way to retrieve ReservationId in Apply?

            return Task.FromResult(0);
        }

        public void Apply(RoomOccupied roomOccupied)
        {
            Publish(new AssignRoom(
                _reservationId,
                roomOccupied.Id,
                roomOccupied.Occupant));
        }

        public void Apply(RoomAssigned aggregateEvent)
        {
            Complete();
        }
    }
}