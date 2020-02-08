using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Infi.DojoEventSourcing.Domain.Rooms.Events;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    [Table("Room")]
    public class RoomReadModel
        : IReadModel,
          IAmReadModelFor<Room, Room.RoomId, RoomCreated>
    {
        [SqlReadModelIgnoreColumn]
        public Room.RoomId Id => Room.RoomId.With(AggregateId);

        public string AggregateId { get; private set; }
        public string RoomNumber { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<Room, Room.RoomId, RoomCreated> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            RoomNumber = domainEvent.AggregateEvent.RoomNumber;
        }
    }
}