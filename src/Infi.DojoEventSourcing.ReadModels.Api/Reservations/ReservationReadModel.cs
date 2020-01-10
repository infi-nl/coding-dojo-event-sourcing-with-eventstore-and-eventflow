using System;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    public class ReservationReadModel
        : IReadModel,
            IAmReadModelFor<Reservation, ReservationId, ReservationPlaced>
    {
        public void Apply(
            IReadModelContext context,
            IDomainEvent<Reservation, ReservationId, ReservationPlaced> domainEvent)
        {
            throw new NotImplementedException();
        }
    }
}