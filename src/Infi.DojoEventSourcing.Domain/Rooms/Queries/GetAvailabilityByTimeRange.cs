using System;
using EventFlow.Queries;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    public class GetAvailabilityByTimeRange : IQuery<RoomAvailabilityDto[]>
    {
        public GetAvailabilityByTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime End { get; }

        public DateTime Start { get; }
    }
}