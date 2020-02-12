using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace Infi.DojoEventSourcing.Domain.Rooms.Events
{
    [EventVersion("RoomCreated", 1)]
    public class RoomCreated : IAggregateEvent<Room, Room.RoomId>
    {
        public RoomCreated(string roomNumber)
        {
            RoomNumber = roomNumber;
        }

        public string RoomNumber { get; }
    }
}