using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Commands
{
    public class PlaceBooking : Command<Booking, BookingId>
    {
        public PlaceBooking(BookingId bookingId)
            : base(bookingId)
        {
        }
    }
}