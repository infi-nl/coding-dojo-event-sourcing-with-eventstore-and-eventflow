using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class AssignRoomHandler : CommandHandler<Reservation, ReservationId, AssignRoom>
    {
        private readonly GetAvailabilityByTimeRangeHandler _getAvailabilityByTimeRangeHandler;

        public AssignRoomHandler(GetAvailabilityByTimeRangeHandler getAvailabilityByTimeRangeHandler)
        {
            _getAvailabilityByTimeRangeHandler = getAvailabilityByTimeRangeHandler;
        }

        public override async Task ExecuteAsync(Reservation reservation, AssignRoom command,
            CancellationToken cancellationToken)
        {
        }
    }
}