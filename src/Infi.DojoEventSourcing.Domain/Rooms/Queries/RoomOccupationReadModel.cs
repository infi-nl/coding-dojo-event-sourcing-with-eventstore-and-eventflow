using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Infi.DojoEventSourcing.Domain.Rooms.Events;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    [Table("RoomOccupation")]
    public class RoomOccupationReadModel
        : IReadModel,
          IAmReadModelFor<Room, Room.RoomId, RoomOccupied>
    {
        [SqlReadModelIdentityColumn]
        public string OccupationId { get; set; }

        [SqlReadModelIgnoreColumn]
        public Room.RoomId Id => Room.RoomId.With(AggregateId);

        public string AggregateId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<Room, Room.RoomId, RoomOccupied> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            StartDate = domainEvent.AggregateEvent.StartDateUtc;
            EndDate = domainEvent.AggregateEvent.EndDateUtc;
        }
    }
}