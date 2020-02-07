using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using Infi.DojoEventSourcing.Domain.Hotels;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;
using Xunit;

namespace Infi.DojoEventSourcing.UnitTests.Domain.Reservations.Commands
{
    public class AssignRoomTests
    {
        [Fact]
        public async Task assigns_the_room_to_the_reservation()
        {
            // Given
            var reservationId = ReservationId.New;
            var reservation = new Reservation(reservationId);
            reservation.Apply(GetValidReservationCreatedEvent(reservationId));

            var roomId = Room.RoomId.New;
            var handler = new AssignRoomHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new AssignRoom(reservationId, roomId),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);
            var uncommittedEvents = reservation.UncommittedEvents.ToArray();
            var assignRoomEvent = (RoomAssigned)uncommittedEvents.Single().AggregateEvent;
            Assert.Equal(roomId, assignRoomEvent.RoomId);
        }

        [Fact]
        public async Task the_assigned_room_can_be_changed()
        {
            // Given
            var reservationId = ReservationId.New;
            var reservation = new Reservation(reservationId);
            var roomId1 = Room.RoomId.New;

            reservation.Apply(GetValidReservationCreatedEvent(reservationId));
            reservation.Apply(new RoomAssigned(reservationId, roomId1));

            var roomId2 = Room.RoomId.New;
            var handler = new AssignRoomHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new AssignRoom(reservationId, roomId2),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);
            var uncommittedEvents = reservation.UncommittedEvents.ToArray();
            var assignRoomEvent = (RoomAssigned)uncommittedEvents.Single().AggregateEvent;
            Assert.Equal(roomId2, assignRoomEvent.RoomId);
        }

        [Fact]
        public async Task cannot_assign_the_room_before_checkin_and_checkout_times_are_known()
        {
            // Given
            var reservationId = ReservationId.New;
            var reservation = new Reservation(reservationId);
            var roomId = Room.RoomId.New;

            var handler = new AssignRoomHandler();

            // When
            var result = await handler.ExecuteCommandAsync(reservation,
                new AssignRoom(reservationId, roomId),
                CancellationToken.None);

            // Then
            Assert.False(result.IsSuccess);
            Assert.Equal(
                $"unexpected state: {Reservation.State.Prospective}",
                ((FailedExecutionResult)result).Errors.Single());
        }

        private ReservationCreated GetValidReservationCreatedEvent(ReservationId id)
        {
            var irrelevantArrivalDate = new DateTime(2020, 02, 01);
            var irrelevantDepartureDate = new DateTime(2020, 02, 02);
            return new ReservationCreated(
                id,
                irrelevantArrivalDate,
                irrelevantDepartureDate,
                Hotel.CreateCheckInTimeFromDate(irrelevantArrivalDate),
                Hotel.CreateCheckOutTimeFromDate(irrelevantDepartureDate));
        }
    }
}