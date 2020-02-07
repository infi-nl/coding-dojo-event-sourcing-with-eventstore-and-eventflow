using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries
{
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