using System;
using System.Threading.Tasks;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    public interface IOfferReadRepository
    {
        Task<ReservationOffer> GetOfferById(
            string reservationId,
            DateTime arrival,
            DateTime departure);
    }
}