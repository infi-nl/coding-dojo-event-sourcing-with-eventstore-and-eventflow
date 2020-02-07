using System;
using System.Collections.Generic;
using EventFlow.Queries;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    public class GetAvailabilityByDateRange : IQuery<IReadOnlyList<RoomAvailabilityDto>>
    {
        public GetAvailabilityByDateRange(DateTime startDateUtc, DateTime endDateUtc)
        {
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
        }

        public DateTime StartDateUtc { get; }
        public DateTime EndDateUtc { get; }
    }
}