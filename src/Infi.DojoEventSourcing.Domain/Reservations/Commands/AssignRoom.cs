using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class AssignRoom : Command<Reservation, ReservationId>
    {
        public AssignRoom(
            ReservationId reservationId,
            Room.RoomId roomId)
            : base(reservationId)
        {
            RoomId = roomId;
        }

        public Room.RoomId RoomId { get; }
    }
}