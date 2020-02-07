using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
{
    public class GetAllReservationsHandler : IQueryHandler<GetAllReservations, IReadOnlyList<ReservationReadModel>>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;

        public GetAllReservationsHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<IReadOnlyList<ReservationReadModel>> ExecuteQueryAsync(
            GetAllReservations query,
            CancellationToken cancellationToken) =>
            await _dbReadContext.RunAsync(factory => factory.CreateReservationRepository().GetAll());
    }
}