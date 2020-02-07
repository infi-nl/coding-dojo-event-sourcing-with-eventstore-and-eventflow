using System;
using EventFlow.Commands;
using EventFlow.Core;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;

namespace Infi.DojoEventSourcing.Domain.Reservations.Commands
{
    public class OccupyAnyAvailableRoom : Command<Reservation, ReservationId>
    {
        public OccupyAnyAvailableRoom(ReservationId aggregateId, DateTime start, DateTime end) : base(aggregateId)
        {
            Start = start;
            End = end;
        }

        public OccupyAnyAvailableRoom(ReservationId aggregateId, DateTime start, DateTime end, ISourceId sourceId) :
            base(aggregateId, sourceId)
        {
            Start = start;
            End = end;
        }

        public DateTime End { get; }

        public DateTime Start { get; }
    }
}