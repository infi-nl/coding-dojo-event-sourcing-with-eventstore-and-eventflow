using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    public class GetAvailabilityByTimeRangeHandler : IQueryHandler<GetAvailabilityByTimeRange, RoomAvailabilityDto[]>
    {
        private IRoomRepository _roomRepository;

        public GetAvailabilityByTimeRangeHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public Task<RoomAvailabilityDto[]> ExecuteQueryAsync(
            GetAvailabilityByTimeRange query,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}