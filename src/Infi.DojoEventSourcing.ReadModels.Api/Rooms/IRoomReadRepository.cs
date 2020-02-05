using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms
{
    public interface IRoomReadRepository
    {
        Task<IReadOnlyList<RoomReadModel>> GetAll();

        Task<IReadOnlyList<RoomOccupationReadModel>> GetOccupationsBetween(DateTime startDate, DateTime endDate);
    }
}