using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
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

        public void Occupy(ReservationId reservationId, Range range, Guid occupant)
        {
            if (IsOccupiedAt(range))
            {
                throw new RoomAlreadyOccupiedException();
            }

            Emit(new RoomOccupied(reservationId, range.Start, range.End, occupant));
        }

        // FIXME ED Check occupied
        private bool IsOccupiedAt(Range range) => false; //  _occupiedRanges.Any(range.Overlaps);

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