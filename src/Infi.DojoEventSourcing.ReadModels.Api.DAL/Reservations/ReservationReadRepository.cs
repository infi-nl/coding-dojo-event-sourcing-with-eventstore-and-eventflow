using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;

namespace Infi.DojoEventSourcing.ReadModels.Api.DAL.Reservations
{
    public class ReservationReadRepository : IReservationReadRepository
    {
        private readonly DbConnection _connection;

        public ReservationReadRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IReadOnlyList<ReservationReadModel>> GetAll()
        {
            var reservations =
                await _connection.QueryAsync<ReservationReadModel>(
                    @"SELECT * FROM Reservation");

            return reservations.ToImmutableList();
        }
    }
}