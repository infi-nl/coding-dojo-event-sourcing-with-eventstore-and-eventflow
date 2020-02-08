using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Events;

namespace DojoEventSourcing
{
    public class RoomOccupationReadModelLocator : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
            if (!(domainEvent is IDomainEvent<Room, Room.RoomId, RoomOccupied>))
            {
                yield break;
            }

            yield return Guid.NewGuid().ToString();
        }
    }
}