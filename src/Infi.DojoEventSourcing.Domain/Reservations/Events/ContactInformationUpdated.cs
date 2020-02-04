using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("ContactInformationUpdated", 1)]
    public class ContactInformationUpdated : AggregateEvent<Reservation, ReservationId>
    {
        public ContactInformationUpdated(ReservationId id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public ReservationId Id { get; }

        public string Name { get; }

        public string Email { get; }
    }
}