using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("ContactInformationUpdated", 1)]
    public class ContactInformationUpdated : AggregateEvent<Reservation, ReservationId>
    {
        public ContactInformationUpdated(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }

        public string Email { get; }
    }
}