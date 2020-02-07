using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;
using Infi.DojoEventSourcing.ReadModels.Api.Rooms;

namespace Infi.DojoEventSourcing.ReadModels.Api.DAL.Rooms
{
    public class RoomReadRepository : IRoomReadRepository
    {
        private readonly DbConnection _connection;

        public RoomReadRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IReadOnlyList<RoomReadModel>> GetAll()
        {
            var rooms = await _connection.QueryAsync<RoomReadModel>(@"SELECT * FROM Room");

            return rooms.ToImmutableList();
        }

        public async Task<IReadOnlyList<RoomOccupationReadModel>> GetOccupationsBetween(
            DateTime startDate,
            DateTime endDate)
        {
            var roomOccupations = await _connection.QueryAsync<RoomOccupationReadModel>(
                @"SELECT * FROM RoomOccupation WHERE EndDate > @StartDate AND StartDate < @EndDate",
                new
                {
                    StartDate = startDate,
                    EndDate = endDate,
                });

            return roomOccupations.ToImmutableList();
        }
    }
}