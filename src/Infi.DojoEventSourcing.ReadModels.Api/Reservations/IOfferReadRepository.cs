using System;
using System.Threading.Tasks;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    public interface IOfferReadRepository
    {
        Task<ReservationOffer> GetAvailableOffersForReservation(ReservationId reservationId,
            DateTime date,
            DateTime expiry);
    }
}