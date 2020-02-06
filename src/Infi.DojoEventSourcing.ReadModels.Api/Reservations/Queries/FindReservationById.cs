using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
{
    public class FindReservationById : IQuery<ReservationReadModel>
    {
        public FindReservationById(ReservationId id)
        {
            ReservationId = id;
        }

        public ReservationId ReservationId { get; }
    }

    public class FindReservationByIdHandler : IQueryHandler<FindReservationById, ReservationReadModel>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public FindReservationByIdHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<ReservationReadModel> ExecuteQueryAsync(
            FindReservationById query,
            CancellationToken cancellationToken) =>
            await _dbReadContext.RunAsync(factory =>
                factory.CreateReservationRepository().GetById(query.ReservationId));
    }
}