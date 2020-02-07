using System.Collections.Generic;
using EventFlow.Queries;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    public class GetAllRooms : IQuery<IReadOnlyList<RoomReadModel>>
    {
    }
}