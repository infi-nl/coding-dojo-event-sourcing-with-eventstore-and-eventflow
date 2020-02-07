using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;
using Infi.DojoEventSourcing.Domain.Rooms.Commands;
using Infi.DojoEventSourcing.Domain.Rooms.Events;
using Xunit;
using Range = Infi.DojoEventSourcing.Domain.Rooms.Range;

namespace Infi.DojoEventSourcing.UnitTests.Domain.Rooms.Commands
{
    public class OccupyRoomTests
    {
        [Fact]
        public async Task occupy_room_succeeds_if_room_was_not_occupied()
        {
            // Given
            var irrelevantRoomId = Room.RoomId.New;
            var irrelevantReservationId = ReservationId.New;
            var irrelevantRange = new Range(
                DateTime.UtcNow.AddDays(-3),
                DateTime.UtcNow.AddDays(3));

            var room = new Room(irrelevantRoomId);
            var handler = new OccupyRoomHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                room,
                new OccupyRoom(irrelevantRoomId, irrelevantReservationId, irrelevantRange),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var uncommittedEvents = room.UncommittedEvents.ToArray();
            Assert.Single(uncommittedEvents);

            Assert.IsType<RoomOccupied>(uncommittedEvents[0].AggregateEvent);
            var roomOccupiedEvent = (RoomOccupied)uncommittedEvents[0].AggregateEvent;

            Assert.Equal(irrelevantRange.Start, roomOccupiedEvent.StartDateUtc);
            Assert.Equal(irrelevantRange.End, roomOccupiedEvent.EndDateUtc);
        }

        [Theory]
        [InlineData("2020-01-15", "2020-01-21", "2020-01-13", "2020-01-17")]
        [InlineData("2020-01-13", "2020-01-17", "2020-01-15", "2020-01-21")]
        [InlineData("2020-01-13", "2020-01-17", "2020-01-12", "2020-01-18")]
        [InlineData("2020-01-13", "2020-01-17", "2020-01-14", "2020-01-16")]
        public async Task occupy_room_fails_if_room_was_already_occupied(
            string requestedDateRangeStartDate,
            string requestedDateRangeEndDate,
            string alreadyOccupiedDateRangeStartDate,
            string alreadyOccupiedDateRangeEndDate)
        {
            // Given
            var irrelevantRoomId = Room.RoomId.New;
            var irrelevantPreviousOccupant = Guid.NewGuid();
            var irrelevantPreviousReservationId = ReservationId.New;
            var irrelevantNewReservationId = ReservationId.New;

            var requestedDateRange = new Range(
                DateTime.ParseExact(requestedDateRangeStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateTime.ParseExact(requestedDateRangeEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            var irrelevantPreviousOccupiedEvent = new RoomOccupied(
                irrelevantPreviousReservationId,
                DateTime.ParseExact(alreadyOccupiedDateRangeStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateTime.ParseExact(alreadyOccupiedDateRangeEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                irrelevantPreviousOccupant);

            var room = new Room(irrelevantRoomId);
            var handler = new OccupyRoomHandler();

            room.Apply(irrelevantPreviousOccupiedEvent);

            // When
            var result = await handler.ExecuteCommandAsync(
                room,
                new OccupyRoom(irrelevantRoomId, irrelevantNewReservationId, requestedDateRange),
                CancellationToken.None);

            // Then
            Assert.False(result.IsSuccess);
            Assert.Empty(room.UncommittedEvents);

            Assert.Equal(
                $"Room already occupied",
                ((FailedExecutionResult)result).Errors.Single());
        }
    }
}