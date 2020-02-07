using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Commands;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
using Xunit;

namespace Infi.DojoEventSourcing.UnitTests.Domain.Rooms.Commands
{
    public class CreateRoomTests
    {
        [Fact]
        public async Task creates_room_with_roomcreated_event_containing_room_number()
        {
            // Given
            var irrelevantRoomId = Room.RoomId.New;
            var irrelevantRoomNumber = "123";

            var room = new Room(irrelevantRoomId);
            var handler = new CreateRoomHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                room,
                new CreateRoom(irrelevantRoomId, irrelevantRoomNumber),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var uncommittedEvents = room.UncommittedEvents.ToArray();
            Assert.Single(uncommittedEvents);

            Assert.IsType<RoomCreated>(uncommittedEvents[0].AggregateEvent);
            var roomCreatedEvent = (RoomCreated)uncommittedEvents[0].AggregateEvent;

            Assert.Equal(irrelevantRoomNumber, roomCreatedEvent.RoomNumber);
        }

        [Fact]
        public void room_created_event_correctly_sets_room_number()
        {
            // Given
            var irrelevantRoomId = Room.RoomId.New;
            var irrelevantRoomNumber = "123";

            var room = new Room(irrelevantRoomId);

            // When
            room.Apply(new RoomCreated(irrelevantRoomNumber));

            // Then
            Assert.Equal(irrelevantRoomNumber, room.RoomNumber);
        }
    }
}