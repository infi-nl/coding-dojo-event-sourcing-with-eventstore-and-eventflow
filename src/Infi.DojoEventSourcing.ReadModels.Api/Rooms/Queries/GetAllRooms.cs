using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms.Queries
{
    public class GetAllRooms : IQuery<IReadOnlyList<RoomReadModel>>
    {
    }

    public class GetAllRoomsHandler : IQueryHandler<GetAllRooms, IReadOnlyList<RoomReadModel>>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetAllRoomsHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<IReadOnlyList<RoomReadModel>> ExecuteQueryAsync(
            GetAllRooms query,
            CancellationToken cancellationToken)
        {
            return await _dbReadContext.RunAsync(factory =>
            {
                var roomRepository = factory.CreateRoomRepository();
                return roomRepository.GetAll();
            });
        }
    }
}