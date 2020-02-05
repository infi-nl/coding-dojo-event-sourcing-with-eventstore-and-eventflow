using EventFlow.Aggregates;

namespace Infi.DojoEventSourcing.Domain.Rooms.Events
{
    public class RoomCreated : IAggregateEvent<Room, Room.RoomIdentity>
    {
        public RoomCreated(string roomNumber)
        {
            RoomNumber = roomNumber;
        }

        public string RoomNumber { get; }
    }
}