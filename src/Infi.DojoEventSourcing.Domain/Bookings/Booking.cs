using EventFlow.Aggregates;
using Infi.DojoEventSourcing.Domain.Bookings.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Bookings
{
    public class Booking : AggregateRoot<Booking, BookingId>
    {
        public Booking(BookingId id)
            : base(id)
        {
        }
    }
}