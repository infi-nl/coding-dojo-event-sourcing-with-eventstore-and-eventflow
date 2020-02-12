using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace DojoEventSourcing
{
    public class OfferReadModelLocator : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
            if (!(domainEvent is IDomainEvent<Reservation, ReservationId, PriceOffered>))
            {
                yield break;
            }

            yield return Guid.NewGuid().ToString();
        }
    }
}