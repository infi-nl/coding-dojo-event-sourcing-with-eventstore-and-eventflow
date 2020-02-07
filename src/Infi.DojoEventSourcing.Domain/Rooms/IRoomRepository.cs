namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public interface IRoomRepository
    {
        void Create(Room.RoomId commandAggregateId);
    }
}