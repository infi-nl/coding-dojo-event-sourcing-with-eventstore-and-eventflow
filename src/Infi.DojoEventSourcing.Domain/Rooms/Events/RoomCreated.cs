using EventFlow.Aggregates;

namespace Infi.DojoEventSourcing.Domain.Rooms.Events
{
    public class RoomCreated : IAggregateEvent, IAggregateEvent<Room, Room.RoomIdentity>
    {
        public RoomCreated(Room.RoomIdentity id, string number)
        {
            throw new System.NotImplementedException();
        }
    }
}