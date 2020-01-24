using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    public interface IReservationReadRepository
    {
        Task<IReadOnlyList<ReservationReadModel>> GetAll();
    }
}