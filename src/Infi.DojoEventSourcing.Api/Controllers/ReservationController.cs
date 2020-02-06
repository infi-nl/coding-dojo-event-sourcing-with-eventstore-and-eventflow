using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Core;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries;
using Microsoft.AspNetCore.Mvc;
using static Infi.DojoEventSourcing.Domain.Reservations.ValueObjects.ReservationId;

namespace DojoEventSourcing.Controllers
{
    [Route("[controller]")]
    public class ReservationController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public ReservationController(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allReservations = await _queryProcessor.ProcessAsync(
                new GetAllReservations(),
                CancellationToken.None);

            return Json(allReservations);
        }

        [HttpGet("Offers")]
        public async Task<OfferDto> FindOffer(
            DateTime arrival,
            DateTime departure)
        {
            var reservationId = Identity<ReservationId>.New;

            await _commandBus
                .PublishAsync(new CreateOffer(reservationId, arrival, departure), CancellationToken.None)
                .ConfigureAwait(false);

            var offer = _queryProcessor
                .Process(new GetOffers(reservationId, arrival, departure), CancellationToken.None);

            return OfferDto.FromReservationOffer(offer);
        }

        public async Task<IActionResult> PlaceReservation([FromBody] PlaceReservationDto reservationDto)
        {
            var id = ReservationId.With(reservationDto.ReservationId);

            var result =
                await _commandBus.PublishAsync(new MakeReservation(
                        id,
                        reservationDto.Name,
                        reservationDto.Email,
                        reservationDto.Arrival,
                        reservationDto.Departure),
                    CancellationToken.None);

            if (result.IsSuccess)
            {
                return Json(id.GetGuid());
            }

            return BadRequest();
        }

        public class PlaceReservationDto
        {
            public Guid ReservationId { get; set; }
            public DateTime Arrival { get; set; }
            public DateTime Departure { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public class OfferDto
        {
            public static OfferDto FromReservationOffer(ReservationOffer offer) => new OfferDto
            {
                ReservationId = Identity<ReservationId>.With(offer.AggregateId).GetGuid(),
                Date = offer.Date,
                Expires = offer.Expires,
                Price = offer.Price
            };

            public Guid ReservationId { get; set; }

            public DateTime Date { get; set; }

            public DateTime Expires { get; set; }

            public decimal Price { get; set; }
        }
    }
}