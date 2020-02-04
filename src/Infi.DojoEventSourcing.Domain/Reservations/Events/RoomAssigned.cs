using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("RoomAssigned", 1)]
    public class RoomAssigned : AggregateEvent<Reservation, ReservationId>
    {
        public RoomAssigned(ReservationId id, Room.RoomIdentity roomId, string roomNumber)
        {
            Id = id;
            RoomId = roomId;
            RoomNumber = roomNumber;
        }

        public ReservationId Id { get; }

        public Room.RoomIdentity RoomId { get; }

        public string RoomNumber { get; }
    }
}