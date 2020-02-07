using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms.Queries
{
    public class GetAllRoomsHandler : IQueryHandler<GetAllRooms, IReadOnlyList<RoomReadModel>>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetAllRoomsHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<IReadOnlyList<RoomReadModel>> ExecuteQueryAsync(
            GetAllRooms query,
            CancellationToken cancellationToken) =>
            await _dbReadContext
                .RunAsync(factory => factory.CreateRoomRepository()
                    .GetAll());
    }
}