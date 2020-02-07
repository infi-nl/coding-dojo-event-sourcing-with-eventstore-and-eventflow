using System.Collections.Generic;
using EventFlow.Queries;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    public class GetAllReservations : IQuery<IReadOnlyList<ReservationReadModel>>
    {
    }
}