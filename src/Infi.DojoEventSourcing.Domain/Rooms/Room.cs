using System.Collections.Generic;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
using Newtonsoft.Json;

namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public class Room : AggregateRoot<Room, Room.RoomId>, IApply<RoomOccupied>, IApply<RoomCreated>
    {
        private readonly IList<Range> _occupiedRanges = new List<Range>();

        [JsonConverter(typeof(SingleValueObjectConverter))]
        public class RoomId : Identity<RoomId>
        {
            public RoomId(string value)
                : base(value)
            {
            }
        }

        public Room(RoomId id)
            : base(id)
        {
        }

        public string RoomNumber { get; set; }

        public void Create(string number)
        {
            Emit(new RoomCreated(number));
        }

        public void Occupy(ReservationId reservationId, Range range)
        {
            if (IsOccupiedAt(range))
            {
                throw new RoomAlreadyOccupiedException($"Room {Id} already occupied");
            }

            Emit(new RoomOccupied(reservationId, range.Start, range.End));
        }

        private bool IsOccupiedAt(Range range) => _occupiedRanges.Any(range.Overlaps);

        public void Apply(RoomOccupied @event)
        {
            _occupiedRanges.Add(new Range(@event.StartDateUtc, @event.EndDateUtc));
        }

        public void Apply(RoomCreated aggregateEvent)
        {
            RoomNumber = aggregateEvent.RoomNumber;
        }
    }
}