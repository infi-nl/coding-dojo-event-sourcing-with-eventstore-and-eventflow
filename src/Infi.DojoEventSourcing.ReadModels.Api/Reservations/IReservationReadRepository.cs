using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    public interface IReservationReadRepository
    {
        Task<IReadOnlyList<ReservationReadModel>> GetAll();
        Task<ReservationReadModel> GetById(ReservationId id);
        Task<IEnumerable<ReservationReadModel>> GetByDate(DateTime date);
        Task<IEnumerable<ReservationReadModel>> GetByRange(DateTime arrival, DateTime departure);
    }
}