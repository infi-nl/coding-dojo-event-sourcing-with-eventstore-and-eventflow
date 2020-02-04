using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Extensions;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.Commands;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations;
using Infi.DojoEventSourcing.ReadModels.Api.Reservations.Queries;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var allReservations = await _queryProcessor.ProcessAsync(
                new GetAllReservations(),
                CancellationToken.None);

            return Json(allReservations);
        }

        [HttpGet("Offers")]
        public async Task<ReservationOffer> ShowOffer(
            DateTime arrival,
            DateTime departure)
        {
            var reservationId = ReservationId.New;
            await _commandBus
                .PublishAsync(new CreateOffer(reservationId, arrival, departure), CancellationToken.None)
                .ConfigureAwait(false);

            return _queryProcessor
                .Process(new GetOffers(reservationId, arrival, departure), CancellationToken.None);
        }


        [HttpPost("{reservationId}")]
        public async Task<IActionResult> PlaceReservation(Guid reservationId)
        {
            var id = ReservationId.With(reservationId);

            var result = await _commandBus.PublishAsync(new MakeReservation(
                    id,
                    "name",
                    "email@example.com",
                    DateTime.Today,
                    DateTime.Today.AddDays(2)),
                CancellationToken.None);

            if (result.IsSuccess)
            {
                return Json(reservationId);
            }

            return BadRequest();
        }
    }
}