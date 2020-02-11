using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Reservations.Events
{
    [EventVersion("LineItemCreated", 1)]
    public class LineItemCreated : AggregateEvent<Reservation, ReservationId>
    {
        public LineItemCreated(ReservationId id, int lineItems, DateTime offerDate, Money offerPrice)
        {
            Id = id;
            LineItems = lineItems;
            OfferDate = offerDate;
            OfferPrice = offerPrice;
        }

        public ReservationId Id { get; }

        public int LineItems { get; }

        public DateTime OfferDate { get; }

        public Money OfferPrice { get; }
    }
}