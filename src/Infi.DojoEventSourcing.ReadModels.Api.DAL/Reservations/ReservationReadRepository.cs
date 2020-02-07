using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Repositories;

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
                await _connection.QueryAsync<ReservationReadModel>("SELECT * FROM Reservation");

            return reservations.ToImmutableList();
        }

        public async Task<ReservationReadModel> GetById(ReservationId id)
        {
            var reservations =
                await _connection.QueryAsync<ReservationReadModel>(
                    "SELECT * FROM Reservation WHERE AggregateId = @ReservationId",
                    new { ReservationId = id.GetGuid().ToString() });

            return reservations.FirstOrDefault();
        }

        public async Task<IEnumerable<ReservationReadModel>> GetByDate(DateTime date) =>
            await _connection.QueryAsync<ReservationReadModel>(
                "SELECT * FROM Reservation WHERE Arrival <= @Date AND Departure >= @Date",
                new { Date = date.Date });

        public async Task<IEnumerable<ReservationReadModel>> GetByRange(DateTime arrival, DateTime departure) =>
            await _connection.QueryAsync<ReservationReadModel>(
                "SELECT * FROM Reservation WHERE Arrival >= @Arrival AND Departure <= @Departure",
                new { Arrival = arrival.Date, Departure = departure.Date });
    }
}