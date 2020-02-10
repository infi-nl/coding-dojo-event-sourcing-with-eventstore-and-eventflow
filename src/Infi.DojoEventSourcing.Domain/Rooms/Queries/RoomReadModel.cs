using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
using Newtonsoft.Json;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    [Table("Room")]
    public class RoomReadModel
        : IReadModel,
          IAmReadModelFor<Room, Room.RoomId, RoomCreated>
    {
        public string AggregateId { get; private set; }
        public string RoomNumber { get; private set; }
        public Room.RoomId GetIdentity() => Room.RoomId.With(AggregateId);

        public void Apply(IReadModelContext context, IDomainEvent<Room, Room.RoomId, RoomCreated> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            RoomNumber = domainEvent.AggregateEvent.RoomNumber;
        }
    }
}