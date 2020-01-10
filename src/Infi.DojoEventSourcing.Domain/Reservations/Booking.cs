using EventFlow.Aggregates;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations
{
    public class Booking : AggregateRoot<Booking, BookingId>
    {
        public Booking(BookingId id)
            : base(id)
        {
        }
    }
}