using System;
using System.Collections.Generic;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
using Newtonsoft.Json;

namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public class Room : AggregateRoot<Room, Room.RoomIdentity>, IApply<RoomOccupied>
    {
        private readonly IList<Range> _occupiedRanges = new List<Range>();

        [JsonConverter(typeof(SingleValueObjectConverter))]
        public class RoomIdentity : Identity<RoomIdentity>
        {
            public RoomIdentity(string value)
                : base(value)
            {
            }
        }

        public Room(RoomIdentity id)
            : base(id)
        {
        }

        public void Create(string number)
        {
            Emit(new RoomCreated(Id, number));
        }

        public void Occupy(Range range, Guid occupant)
        {
            if (IsOccupiedAt(range))
            {
                throw new RoomAlreadyOccupiedException();
            }

            Emit(new RoomOccupied(Id, range.Start, range.End, occupant));
        }

        private bool IsOccupiedAt(Range range) => _occupiedRanges.Any(range.Overlaps);

        void IApply<RoomOccupied>.Apply(RoomOccupied @event)
        {
            _occupiedRanges.Add(new Range(@event.Start, @event.End));
        }
    }
}