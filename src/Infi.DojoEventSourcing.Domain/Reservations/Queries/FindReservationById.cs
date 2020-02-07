using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    public class FindReservationById : IQuery<ReservationReadModel>
    {
        public FindReservationById(ReservationId id)
        {
            ReservationId = id;
        }

        public ReservationId ReservationId { get; }
    }
}