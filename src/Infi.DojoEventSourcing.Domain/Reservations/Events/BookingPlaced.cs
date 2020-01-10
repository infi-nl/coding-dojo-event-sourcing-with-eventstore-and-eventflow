using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("BookingPlaced", 1)]
    public class BookingPlaced : AggregateEvent<Booking, BookingId>
    {
    }
}