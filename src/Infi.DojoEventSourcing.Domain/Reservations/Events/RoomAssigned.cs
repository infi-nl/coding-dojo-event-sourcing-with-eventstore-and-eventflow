using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("RoomAssigned", 1)]
    public class RoomAssigned : AggregateEvent<Reservation, ReservationId>
    {
        public RoomAssigned(ReservationId id, Room.RoomIdentity roomId)
        {
            Id = id;
            RoomId = roomId;
        }

        public ReservationId Id { get; }

        public Room.RoomIdentity RoomId { get; }
    }
}