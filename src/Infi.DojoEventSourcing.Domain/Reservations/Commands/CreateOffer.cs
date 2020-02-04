using System;
using EventFlow.Commands;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    [EventVersion("CreateOffer", 1)]
    public class CreateOffer : Command<Reservation, ReservationId>
    {
        public CreateOffer(ReservationId id, in DateTime arrival, in DateTime departure) : base(id)
        {
            Id = id;
            Arrival = arrival;
            Departure = departure;
        }

        public ReservationId Id { get; }

        public DateTime Arrival { get; }

        public DateTime Departure { get; }
    }
}