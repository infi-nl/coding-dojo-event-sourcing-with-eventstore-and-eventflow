using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms
{
    public interface IRoomReadRepository
    {
        Task<IReadOnlyList<RoomReadModel>> GetAll();

        Task<IReadOnlyList<RoomOccupationReadModel>> GetOccupationsBetween(DateTime startDate, DateTime endDate);
    }
}