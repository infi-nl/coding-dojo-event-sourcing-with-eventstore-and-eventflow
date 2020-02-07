using EventFlow.Commands;
using EventFlow.Core;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Rooms.Commands
{
    public class OccupyRoom : Command<Room, Room.RoomId>
    {
        public OccupyRoom(Room.RoomId aggregateId, ReservationId reservationId, Range range) : base(aggregateId)
        {
            ReservationId = reservationId;
            Range = range;
        }

        public OccupyRoom(Room.RoomId aggregateId, Range range, ISourceId sourceId) : base(aggregateId, sourceId)
        {
            Range = range;
        }

        public Range Range;
        public ReservationId ReservationId;
    }
}