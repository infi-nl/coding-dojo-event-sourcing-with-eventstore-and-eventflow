using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infi.DojoEventSourcing.Domain.Pricings;
using Infi.DojoEventSourcing.Domain.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using LanguageExt;
using Moq;
using NodaMoney;
using Xunit;

namespace Infi.DojoEventSourcing.UnitTests.Domain.Reservations.Commands
{
    public class CreateOfferTests
    {
        [Fact]
        public async Task creates_offer_for_every_day_of_staying_at_hote()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var date2 = new DateTime(2020, 02, 02);
            var departureDate = new DateTime(2020, 02, 03);

            var price1 = Money.Euro(101);
            var price2 = Money.Euro(102);
            var irrelevantPrice3 = Money.Euro(103);

            var mockPriceEngine = new Mock<IPricingEngine>();
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, arrivalDate, price1);
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, date2, price2);
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, departureDate, irrelevantPrice3);

            var offersShouldExpireOn = DateTime.UtcNow.AddMinutes(30);

            var handler = new CreateOfferHandler(mockPriceEngine.Object);

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new CreateOffer(id, arrivalDate, departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var uncommittedEvents = reservation.UncommittedEvents.ToArray();
            Assert.Equal(2, uncommittedEvents.Length);

            var firstPriceOffered = (PriceOffered)uncommittedEvents[0].AggregateEvent;
            Assert.Equal(arrivalDate, firstPriceOffered.Date);
            Assert.Equal(price1, firstPriceOffered.Price);
            Assert.InRange(firstPriceOffered.Expires, offersShouldExpireOn, offersShouldExpireOn.AddSeconds(1));

            var secondPriceOffered = (PriceOffered)uncommittedEvents[1].AggregateEvent;
            Assert.Equal(date2, secondPriceOffered.Date);
            Assert.Equal(price2, secondPriceOffered.Price);
            Assert.InRange(secondPriceOffered.Expires, offersShouldExpireOn, offersShouldExpireOn.AddSeconds(1));
        }

        [Fact]
        public async Task if_price_not_available_then_does_not_record_a_price_offer()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var departureDate = new DateTime(2020, 02, 02);

            var mockPriceEngine = new Mock<IPricingEngine>();
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, arrivalDate, Option<Money>.None);

            var handler = new CreateOfferHandler(mockPriceEngine.Object);

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new CreateOffer(id, arrivalDate, departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);
            Assert.Empty(reservation.UncommittedEvents);
        }

        [Fact]
        public async Task subsequent_requests_offer_only_prices_for_new_dates()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var date2 = new DateTime(2020, 02, 02);
            var departureDate = new DateTime(2020, 02, 03);
            var irrelevantOfferedPrice1 = Money.Euro(101);
            var irrelevantOfferedPrice2 = Money.Euro(101);

            reservation.Apply(
                new PriceOffered(id, arrivalDate, irrelevantOfferedPrice1, DateTime.UtcNow.AddMinutes(30)));

            var mockPriceEngine = new Mock<IPricingEngine>();
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, arrivalDate, irrelevantOfferedPrice1);
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, date2, irrelevantOfferedPrice2);

            var handler = new CreateOfferHandler(mockPriceEngine.Object);

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new CreateOffer(id, arrivalDate, departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var priceOffered = (PriceOffered)reservation.UncommittedEvents.Single().AggregateEvent;
            Assert.Equal(date2, priceOffered.Date);
        }

        [Fact]
        public async Task if_old_price_offer_has_expired_then_produces_a_new_price_offer()
        {
            // Given
            var id = ReservationId.New;
            var reservation = new Reservation(id);

            var arrivalDate = new DateTime(2020, 02, 01);
            var departureDate = new DateTime(2020, 02, 03);
            var offeredPrice = Money.Euro(101);
            var newPrice = Money.Euro(102);

            reservation.Apply(new PriceOffered(id, arrivalDate, offeredPrice, DateTime.UtcNow.AddSeconds(-1)));

            var mockPriceEngine = new Mock<IPricingEngine>();
            SetupPriceEngineReturnsPriceForDate(mockPriceEngine, arrivalDate, newPrice);

            var newOfferShouldExpireOn = DateTime.UtcNow.AddMinutes(30);

            var handler = new CreateOfferHandler(mockPriceEngine.Object);

            // When
            var result = await handler.ExecuteCommandAsync(
                reservation,
                new CreateOffer(id, arrivalDate, departureDate),
                CancellationToken.None);

            // Then
            Assert.True(result.IsSuccess);

            var uncommittedEvents = reservation.UncommittedEvents.ToArray();

            var newPriceOffered = (PriceOffered)uncommittedEvents.Single().AggregateEvent;
            Assert.Equal(arrivalDate, newPriceOffered.Date);
            Assert.Equal(newPrice, newPriceOffered.Price);

            Assert.InRange(newPriceOffered.Expires, newOfferShouldExpireOn, newOfferShouldExpireOn.AddSeconds(1));
        }

        private static void SetupPriceEngineReturnsPriceForDate(
            Mock<IPricingEngine> mockPriceEngine,
            DateTime date,
            Option<Money> price)
        {
            mockPriceEngine
                .Setup(e => e.GetAccommodationPrice(date))
                .Returns(price);
        }
    }
}