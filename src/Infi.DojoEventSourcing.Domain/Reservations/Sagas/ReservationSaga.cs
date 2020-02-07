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
            ISagaHandles<Reservation, ReservationId, RoomOccupyRequested>,
            ISagaHandles<Room, Room.RoomId, RoomOccupied>,
            ISagaHandles<Reservation, ReservationId, RoomAssigned>,
            IApply<ReservationSaga.StartedReservationSaga>
    {
        private ReservationId _reservationId;

        public ReservationSaga(ReservationSagaId id)
            : base(id)
        {
        }

        public Task HandleAsync(
            IDomainEvent<Reservation, ReservationId, ReservationCreated> domainEvent,
            ISagaContext sagaContext,
            CancellationToken cancellationToken)
        {
            Emit(new StartedReservationSaga(domainEvent.AggregateIdentity));

            Publish(new OccupyAnyAvailableRoom(
                domainEvent.AggregateIdentity,
                domainEvent.AggregateEvent.CheckInTime,
                domainEvent.AggregateEvent.CheckOutTime));

            return Task.FromResult(0);
        }

        public Task HandleAsync(IDomainEvent<Reservation, ReservationId, RoomOccupyRequested> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Publish(new OccupyRoom(
                domainEvent.AggregateEvent.RoomId,
                _reservationId,
                new Range(domainEvent.AggregateEvent.Arrival, domainEvent.AggregateEvent.Departure)));

            return Task.FromResult(0);
        }

        public Task HandleAsync(IDomainEvent<Room, Room.RoomId, RoomOccupied> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Publish(new AssignRoom(_reservationId, domainEvent.AggregateIdentity));

            return Task.FromResult(0);
        }

        public Task HandleAsync(IDomainEvent<Reservation, ReservationId, RoomAssigned> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            Complete();

            return Task.FromResult(0);
        }

        public void Apply(StartedReservationSaga startedReservationSaga)
        {
            _reservationId = startedReservationSaga.ReservationIdentity;
        }

        public class StartedReservationSaga : IAggregateEvent<ReservationSaga, ReservationSagaId>
        {
            public StartedReservationSaga(ReservationId reservationIdentity)
            {
                ReservationIdentity = reservationIdentity;
            }

            public ReservationId ReservationIdentity { get; }
        }
    }
}