using System;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class CreateOffer : Command<Reservation, ReservationId>
    {
        public CreateOffer(ReservationId id, DateTime arrival, DateTime departure) : base(id)
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