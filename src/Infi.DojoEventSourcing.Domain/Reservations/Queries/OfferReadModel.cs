﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    [Table("Offer")]
    public class OfferReadModel
        : IReadModel,
            IAmReadModelFor<Reservation, ReservationId, PriceOffered>
    {
        public string AggregateId { get; private set; }
        public string ReservationId { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime Expires { get; private set; }
        public decimal Price { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<Reservation, ReservationId, PriceOffered> domainEvent)
        {
            AggregateId = domainEvent.AggregateEvent.AggregateId;
            ReservationId = domainEvent.AggregateIdentity.GetGuid().ToString();
            Date = domainEvent.AggregateEvent.Date;
            Expires = domainEvent.AggregateEvent.Expires;
            Price = domainEvent.AggregateEvent.Price.Amount;
        }
    }
}