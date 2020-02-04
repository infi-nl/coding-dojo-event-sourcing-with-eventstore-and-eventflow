using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;

namespace Infi.DojoEventSourcing.ReadModels.Api
{
    public interface IApiReadModelRepositoryFactory : IRepositoryFactory
    {
        IReservationReadRepository CreateReservationRepository();
        IOfferReadRepository CreateOffersRepository();
    }
}