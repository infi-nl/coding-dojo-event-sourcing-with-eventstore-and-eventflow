using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;

namespace Infi.DojoEventSourcing.ReadModels.Api.Capacity.Queries
{
    public static class CapacityUtils
    {
        public static IReadOnlyDictionary<DateTime, int> BuildReservationCountLookup(
            IEnumerable<ReservationReadModel> reservations) =>
            reservations
                .SelectMany(_ => BuildDatesFrom(_.Arrival, _.Departure))
                .GroupBy(_ => _)
                .ToImmutableDictionary(_ => _.Key, _ => _.Count());

        public static IEnumerable<DateTime> BuildDatesFrom(DateTime start, DateTime end) =>
            Enumerable
                .Range(0, end.Subtract(start).Days)
                .Select(offset => start.AddDays(offset));

        public static CapacityDto MapToCapacity(DateTime date, IReadOnlyDictionary<DateTime, int> reservation) =>
            new CapacityDto
            {
                Date = date,
                Capacity = 42, // FIXME ED Read actual capacity
                Reserved = reservation.GetValueOrDefault(date)
            };
    }
}