using System;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class AssignRoom : Command<Reservation, ReservationId>
    {
        public AssignRoom(
            ReservationId reservationId,
            Room.RoomIdentity roomIdentity,
            Guid occupant) : base(reservationId)
        {
            ReservationId = ReservationId;
            RoomId = roomIdentity;
            Occupant = occupant;
        }

        public ReservationId ReservationId { get; }

        public Room.RoomIdentity RoomId { get; }

        public Guid Occupant { get; }
    }
}