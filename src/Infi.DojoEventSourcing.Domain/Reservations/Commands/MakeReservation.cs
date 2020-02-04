using System;
using EventFlow.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class MakeReservation : Command<Reservation, ReservationId>
    {
        public MakeReservation(
            ReservationId aggregateId,
            string name,
            string email,
            DateTime arrival,
            DateTime departure)
            : base(aggregateId)
        {
            Name = name;
            Email = email;
            Arrival = arrival;
            Departure = departure;
        }

        public string Name { get; }

        public string Email { get; }

        public DateTime Arrival { get; }

        public DateTime Departure { get; }
    }
}