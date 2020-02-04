using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    [Table("Reservation")]
    public class ReservationReadModel
        : IReadModel,
            IAmReadModelFor<Reservation, ReservationId, ReservationCreated>,
            IAmReadModelFor<Reservation, ReservationId, ContactInformationUpdated>,
            IAmReadModelFor<Reservation, ReservationId, RoomAssigned>
    {
        public string AggregateId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Status { get; set; }
        public string RoomNumber { get; set; }
        public string RoomId { get; set; }
        public DateTime CheckOutTime { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<Reservation, ReservationId, ReservationCreated> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.GetGuid().ToString();
            Arrival = domainEvent.AggregateEvent.Arrival;
            Departure = domainEvent.AggregateEvent.Departure;
            CheckInTime = domainEvent.AggregateEvent.CreateCheckInTimeFromDate;
            CheckOutTime = domainEvent.AggregateEvent.CreateCheckOutTimeFromDate;
            Status = "initiated";
        }

        public void Apply(IReadModelContext context,
            IDomainEvent<Reservation, ReservationId, ContactInformationUpdated> domainEvent)
        {
            Name = domainEvent.AggregateEvent.Name;
            Email = domainEvent.AggregateEvent.Email;
        }

        public void Apply(IReadModelContext context, IDomainEvent<Reservation, ReservationId, RoomAssigned> domainEvent)
        {
            RoomId = domainEvent.AggregateEvent.RoomId.Value;
            RoomNumber = domainEvent.AggregateEvent.RoomNumber;
        }
    }
}