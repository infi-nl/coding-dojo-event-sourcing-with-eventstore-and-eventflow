using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
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
        public string RoomId { get; set; }
        public DateTime CheckOutTime { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public ReservationId GetIdentity() => ReservationId.With(AggregateId);

        public void Apply(
            IReadModelContext context,
            IDomainEvent<Reservation, ReservationId, ReservationCreated> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Arrival = domainEvent.AggregateEvent.Arrival;
            Departure = domainEvent.AggregateEvent.Departure;
            CheckInTime = domainEvent.AggregateEvent.CheckInTime;
            CheckOutTime = domainEvent.AggregateEvent.CheckOutTime;
            Status = Reservation.State.Reserved.ToString();
        }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<Reservation, ReservationId, ContactInformationUpdated> domainEvent)
        {
            Name = domainEvent.AggregateEvent.Name;
            Email = domainEvent.AggregateEvent.Email;
        }

        public void Apply(IReadModelContext context, IDomainEvent<Reservation, ReservationId, RoomAssigned> domainEvent)
        {
            RoomId = domainEvent.AggregateEvent.RoomId.Value;
        }
    }
}