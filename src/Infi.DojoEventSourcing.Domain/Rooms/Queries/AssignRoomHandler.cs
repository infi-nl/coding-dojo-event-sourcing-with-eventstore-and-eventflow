using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    public class AssignRoomHandler : CommandHandler<Reservation, ReservationId, AssignRoom>
    {
        public override async Task ExecuteAsync(Reservation reservation, AssignRoom command,
            CancellationToken cancellationToken)
        {
        }
    }
}