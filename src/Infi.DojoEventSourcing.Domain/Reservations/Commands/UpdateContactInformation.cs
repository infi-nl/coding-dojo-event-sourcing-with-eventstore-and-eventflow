using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class UpdateContactInformation : Command<Reservation, ReservationId>
    {
        public UpdateContactInformation(ReservationId aggregateId, string name, string email)
            : base(aggregateId)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }

        public string Email { get; }
    }
}