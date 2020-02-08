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
using NodaMoney;
using Xunit;

namespace Infi.DojoEventSourcing.UnitTests.Domain.Reservations.Commands
{
    public class MakeReservationHandlerTests
    {
        private const string ValidName = "John Doe";
        private const string ValidEmailAddress = "john@example.com";

        [Fact]
        public async Task make_reservation_for_one_night()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var departureDate = new DateTime(2020, 02, 02);

            var offeredPrice = Money.Euro(101);
            var offerExpiresInFuture = DateTime.UtcNow.AddHours(1);

            reservation.Apply(new PriceOffered(arrivalDate, offeredPrice, offerExpiresInFuture));

            var handler = new MakeReservationHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new MakeReservation(
                    id,
                    ValidName,
                    ValidEmailAddress,
                    arrivalDate,
                    departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);
            Assert.Collection(reservation.UncommittedEvents,
                @event =>
                {
                    var aggregateEvent = (ContactInformationUpdated)@event.AggregateEvent;
                    Assert.Equal(ValidName, aggregateEvent.Name);
                    Assert.Equal(ValidEmailAddress, aggregateEvent.Email);
                },
                @event =>
                {
                    var aggregateEvent = (ReservationCreated)@event.AggregateEvent;
                    Assert.Equal(arrivalDate, aggregateEvent.Arrival);
                    Assert.Equal(departureDate, aggregateEvent.Departure);
                    Assert.Equal(Hotel.CreateCheckInTimeFromDate(arrivalDate), aggregateEvent.CheckInTime);
                    Assert.Equal(Hotel.CreateCheckOutTimeFromDate(departureDate), aggregateEvent.CheckOutTime);
                },
                @event =>
                {
                    var aggregateEvent = (LineItemCreated)@event.AggregateEvent;
                    Assert.Equal(arrivalDate, aggregateEvent.OfferDate);
                    Assert.Equal(offeredPrice, aggregateEvent.OfferPrice);
                    Assert.Equal(1, aggregateEvent.LineItems);
                });
        }

        [Fact]
        public async Task cannot_make_reservation_twice()
        {
            // Given
            var irrelevantArrivalDate = new DateTime(2020, 02, 01);
            var irrelevantDepartureDate = new DateTime(2020, 02, 02);

            var id = ReservationId.New;
            var reservation = new Reservation(id);
            reservation.Apply(new ReservationCreated(
                id,
                irrelevantArrivalDate,
                irrelevantDepartureDate,
                Hotel.CreateCheckInTimeFromDate(irrelevantArrivalDate),
                Hotel.CreateCheckOutTimeFromDate(irrelevantDepartureDate)));

            var handler = new MakeReservationHandler();

            // When
            var result = await handler.ExecuteCommandAsync(reservation,
                new MakeReservation(id,
                    ValidName,
                    ValidEmailAddress,
                    irrelevantArrivalDate,
                    irrelevantDepartureDate),
                CancellationToken.None);

            //Then
            Assert.False(result.IsSuccess);
            Assert.Equal("unexpected state: Reserved", ((FailedExecutionResult)result).Errors.Single());
        }

        [Fact]
        public async Task produces_line_items_for_every_date_in_range()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var offeredPrice1 = Money.Euro(101);
            var offeredPrice2 = Money.Euro(102);
            var offeredPrice3 = Money.Euro(103);
            var offerExpiresInFuture = DateTime.UtcNow.AddHours(1);

            var arrivalDate = new DateTime(2020, 02, 01);
            var date2 = new DateTime(2020, 02, 02);
            var date3 = new DateTime(2020, 02, 03);
            var departureDate = new DateTime(2020, 02, 04);

            reservation.Apply(new PriceOffered(arrivalDate, offeredPrice1, offerExpiresInFuture));
            reservation.Apply(new PriceOffered(date2, offeredPrice2, offerExpiresInFuture));
            reservation.Apply(new PriceOffered( date3, offeredPrice3, offerExpiresInFuture));

            var handler = new MakeReservationHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new MakeReservation(
                    id,
                    ValidName,
                    ValidEmailAddress,
                    arrivalDate,
                    departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var uncommittedEvents = reservation.UncommittedEvents.ToArray();
            var firstLineItem = (LineItemCreated)uncommittedEvents[2].AggregateEvent;
            Assert.Equal(offeredPrice1, firstLineItem.OfferPrice);
            Assert.Equal(arrivalDate, firstLineItem.OfferDate);
            Assert.Equal(1, firstLineItem.LineItems);

            var secondLineItem = (LineItemCreated)uncommittedEvents[3].AggregateEvent;
            Assert.Equal(offeredPrice2, secondLineItem.OfferPrice);
            Assert.Equal(date2, secondLineItem.OfferDate);
            Assert.Equal(2, secondLineItem.LineItems);

            var thirdLineItem = (LineItemCreated)uncommittedEvents[4].AggregateEvent;
            Assert.Equal(offeredPrice3, thirdLineItem.OfferPrice);
            Assert.Equal(date3, thirdLineItem.OfferDate);
            Assert.Equal(3, thirdLineItem.LineItems);
        }

        [Fact]
        public async Task rejects_if_a_price_offer_is_missing()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var date2 = new DateTime(2020, 02, 02);
            var date3 = new DateTime(2020, 02, 03);
            var departureDate = new DateTime(2020, 02, 04);

            var offeredPrice1 = Money.Euro(101);
            var offeredPrice2 = Money.Euro(102);
            var offerExpiresInFuture = DateTime.UtcNow.AddHours(1);

            reservation.Apply(new PriceOffered( arrivalDate, offeredPrice1, offerExpiresInFuture));
            reservation.Apply(new PriceOffered( date2, offeredPrice2, offerExpiresInFuture));

            var handler = new MakeReservationHandler();

            // When
            var result = await handler.ExecuteCommandAsync(reservation,
                new MakeReservation(id,
                    ValidName,
                    ValidEmailAddress,
                    arrivalDate,
                    departureDate),
                CancellationToken.None);

            //Then
            Assert.False(result.IsSuccess);
            Assert.Equal($"no price offer for date {date3}", ((FailedExecutionResult)result).Errors.Single());
        }

        [Fact]
        public async Task rejects_if_a_price_offer_has_expired()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var date2 = new DateTime(2020, 02, 02);
            var departureDate = new DateTime(2020, 02, 03);

            var offeredPrice1 = Money.Euro(101);
            var offeredPrice2 = Money.Euro(102);
            var offerExpiresInFuture = DateTime.UtcNow.AddHours(1);
            var offerExpiresNow = DateTime.UtcNow;

            reservation.Apply(new PriceOffered( arrivalDate, offeredPrice1, offerExpiresInFuture));
            reservation.Apply(new PriceOffered(date2, offeredPrice2, offerExpiresNow));

            var handler = new MakeReservationHandler();

            // When
            var result = await handler.ExecuteCommandAsync(reservation,
                new MakeReservation(id,
                    ValidName,
                    ValidEmailAddress,
                    arrivalDate,
                    departureDate),
                CancellationToken.None);

            //Then
            Assert.False(result.IsSuccess);
            Assert.Equal($"price offer for date {date2} has expired", ((FailedExecutionResult)result).Errors.Single());
        }

        [Fact]
        public async Task uses_the_new_price_if_an_expired_price_offer_has_been_replaced()
        {
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var date2 = new DateTime(2020, 02, 02);
            var departureDate = new DateTime(2020, 02, 03);

            var offeredPrice1 = Money.Euro(101);
            var offeredPrice2 = Money.Euro(102);
            var newOfferedPrice2 = Money.Euro(111);

            var offerExpiresInFuture = DateTime.UtcNow.AddHours(1);
            var offerExpiresNow = DateTime.UtcNow;

            reservation.Apply(new PriceOffered(arrivalDate, offeredPrice1, offerExpiresInFuture));
            reservation.Apply(new PriceOffered(date2, offeredPrice2, offerExpiresNow));
            reservation.Apply(new PriceOffered(date2, newOfferedPrice2, offerExpiresInFuture));

            var handler = new MakeReservationHandler();

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new MakeReservation(
                    id,
                    ValidName,
                    ValidEmailAddress,
                    arrivalDate,
                    departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var uncommittedEvents = reservation.UncommittedEvents.ToArray();
            var secondLineItem = (LineItemCreated)uncommittedEvents[3].AggregateEvent;
            Assert.Equal(newOfferedPrice2, secondLineItem.OfferPrice);
            Assert.Equal(date2, secondLineItem.OfferDate);
        }
    }
}