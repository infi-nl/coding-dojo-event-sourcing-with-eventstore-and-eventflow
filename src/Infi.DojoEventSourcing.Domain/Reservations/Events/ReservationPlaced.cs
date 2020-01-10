using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("ReservationPlaced", 1)]
    public class ReservationPlaced : AggregateEvent<Reservation, ReservationId>
    {
    }
}