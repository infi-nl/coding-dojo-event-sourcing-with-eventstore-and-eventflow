using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms.Queries
{
    public class GetAvailabilityByTimeRangeHandler : IQueryHandler<GetAvailabilityByTimeRange, RoomAvailabilityDto[]>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetAvailabilityByTimeRangeHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<RoomAvailabilityDto[]> ExecuteQueryAsync(
            GetAvailabilityByTimeRange query,
            CancellationToken cancellationToken)
        {
            // FIXME Actually use query parameters to filter
            var rooms = await _dbReadContext
                .RunAsync(f => f.CreateRoomRepository().GetAll());

            return rooms.Select(MapToRoomAvailabilityDto).ToArray();
        }

        private static RoomAvailabilityDto MapToRoomAvailabilityDto(RoomReadModel roomReadModel) =>
            new RoomAvailabilityDto
            {
                Details = new List<RoomAvailabilityIntervalDto>(), // FIXME ED Return actual intervals
                IsAvailable = true,
                RoomId = roomReadModel.GetIdentity().GetGuid(),
                RoomNumber = roomReadModel.RoomNumber
            };
    }
}