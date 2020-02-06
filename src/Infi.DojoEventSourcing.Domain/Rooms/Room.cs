using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
using LanguageExt.TypeClasses;
using Newtonsoft.Json;

namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public class Room : AggregateRoot<Room, Room.RoomIdentity>, IApply<RoomOccupied>, IApply<RoomCreated>
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

        public string RoomNumber { get; set; }

        public void Create(string number)
        {
            Emit(new RoomCreated(number));
        }

        public void Occupy(Range range, Guid occupant)
        {
            if (IsOccupiedAt(range))
            {
                throw new RoomAlreadyOccupiedException();
            }

            Emit(new RoomOccupied(range.Start, range.End, occupant));
        }

        private bool IsOccupiedAt(Range range) => _occupiedRanges.Any(range.Overlaps);

        void IApply<RoomOccupied>.Apply(RoomOccupied @event)
        {
            _occupiedRanges.Add(new Range(@event.StartDateUtc, @event.EndDateUtc));
        }

        public void Apply(RoomCreated aggregateEvent)
        {
            RoomNumber = aggregateEvent.RoomNumber;
        }
    }
}