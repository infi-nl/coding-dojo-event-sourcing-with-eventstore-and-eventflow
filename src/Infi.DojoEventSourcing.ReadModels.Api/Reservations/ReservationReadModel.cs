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
            IAmReadModelFor<Reservation, ReservationId, ReservationCreated>
    {
        public string AggregateId { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<Reservation, ReservationId, ReservationCreated> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.GetGuid().ToString();
        }
    }
}