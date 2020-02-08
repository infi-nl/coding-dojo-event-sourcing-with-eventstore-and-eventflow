using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Core;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries;
using Microsoft.AspNetCore.Mvc;
using TaskExtensions = LanguageExt.TaskExtensions;

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
            var allReservations = await _queryProcessor.ProcessAsync(new GetAllReservations(), CancellationToken.None);

            return Json(allReservations);
        }

        [HttpGet("New")]
        public async Task<ActionResult> GetNewReservation()
        {
            return await Task.FromResult(Json(ReservationId.New.GetGuid()));
        }

        [HttpGet("Offers")]
        public async Task<IActionResult> FindOffer(
            [Required] Guid reservationId,
            [Required] DateTime arrival,
            [Required] DateTime departure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = ReservationId.With(reservationId);
            await _commandBus
                .PublishAsync(new CreateOffer(id, arrival, departure), CancellationToken.None)
                .ConfigureAwait(false);

            var offer = _queryProcessor
                .Process(new GetOffers(id, arrival, departure), CancellationToken.None);

            return Json(OfferDto.FromReservationOffer(offer));
        }

        [HttpPost]
        public async Task<IActionResult> PlaceReservation([FromBody] PlaceReservationDto reservationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("{reservationId}")]
        public ReservationReadModel GetReservationById(Guid reservationId)
        {
            var id = ReservationId.With(reservationId);

            var reservation = _queryProcessor
                .Process(new FindReservationById(id), CancellationToken.None);

            return reservation;
        }

        [HttpPost("UpdateContactInformation")]
        public async Task<IActionResult> UpdateContactInformation(
            [FromBody] ContactInformationDto newContactInformation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = ReservationId.With(newContactInformation.ReservationId);

            var result =
                await _commandBus.PublishAsync(
                    new UpdateContactInformation(
                        id,
                        newContactInformation.Name,
                        newContactInformation.Email),
                    CancellationToken.None);

            if (result.IsSuccess)
            {
                return NoContent();
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

        public class ContactInformationDto
        {
            public Guid ReservationId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public class OfferDto
        {
            public static OfferDto FromReservationOffer(ReservationOffer offer) => new OfferDto
            {
                Date = offer.Date,
                Expires = offer.Expires,
                Price = offer.Price
            };

            public DateTime Date { get; set; }

            public DateTime Expires { get; set; }

            public decimal Price { get; set; }
        }
    }
}