using System.Data.Common;
using Infi.DojoEventSourcing.ReadModels.Api.DAL.Reservations;
using Infi.DojoEventSourcing.ReadModels.Api.DAL.Rooms;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Repositories;
using Infi.DojoEventSourcing.ReadModels.Api.Rooms;

namespace Infi.DojoEventSourcing.ReadModels.Api.DAL
{
    public class ApiReadModelRepositoryFactory : IApiReadModelRepositoryFactory
    {
        private readonly DbConnection _connection;

        public ApiReadModelRepositoryFactory(DbConnection connection)
        {
            _connection = connection;
        }

        public IReservationReadRepository CreateReservationRepository()
        {
            return new ReservationReadRepository(_connection);
        }

        public IRoomReadRepository CreateRoomRepository()
        {
            return new RoomReadRepository(_connection);
        }

        public IOfferReadRepository CreateOffersRepository()
        {
            return new OfferReadRepository(_connection);
        }
    }
}