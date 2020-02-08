using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    [Table("Offer")]
    public class OfferReadModel
        : IReadModel,
          IAmReadModelFor<Reservation, ReservationId, PriceOffered>
    {
        [SqlReadModelIdentityColumn]
        public string OfferId { get; set; }

        [SqlReadModelIgnoreColumn]
        public ReservationId ReservationId => ReservationId.With(AggregateId);

        public string AggregateId { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime Expires { get; private set; }
        public decimal Price { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<Reservation, ReservationId, PriceOffered> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Date = domainEvent.AggregateEvent.Date;
            Expires = domainEvent.AggregateEvent.Expires;
            Price = domainEvent.AggregateEvent.Price.Amount;
        }
    }
}