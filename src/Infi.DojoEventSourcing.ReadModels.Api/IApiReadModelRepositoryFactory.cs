using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Repositories;
using Infi.DojoEventSourcing.ReadModels.Api.Rooms;

namespace Infi.DojoEventSourcing.ReadModels.Api
{
    public interface IApiReadModelRepositoryFactory : IRepositoryFactory
    {
        IReservationReadRepository CreateReservationRepository();
        IOfferReadRepository CreateOffersRepository();
        IRoomReadRepository CreateRoomRepository();
    }
}