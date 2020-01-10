using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Commands.Reservations
{
    public class PlaceReservation : Command<Reservation, ReservationId>
    {
        public PlaceReservation(ReservationId reservationId)
            : base(reservationId)
        {
        }
    }
}