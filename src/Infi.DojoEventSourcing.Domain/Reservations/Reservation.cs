using EventFlow.Aggregates;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations
{
    public class Reservation : AggregateRoot<Reservation, ReservationId>
    {
        public Reservation(ReservationId id)
            : base(id)
        {
        }
    }
}