using EventFlow.Aggregates;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations
{
    public class Reservation
        : AggregateRoot<Reservation, ReservationId>,
            IEmit<ReservationPlaced>,
            IApply<ReservationPlaced>
    {
        public Reservation(ReservationId id)
            : base(id)
        {
        }

        public void Place()
        {
            Emit(new ReservationPlaced());
        }

        public void Apply(ReservationPlaced aggregateEvent)
        {
        }
    }
}