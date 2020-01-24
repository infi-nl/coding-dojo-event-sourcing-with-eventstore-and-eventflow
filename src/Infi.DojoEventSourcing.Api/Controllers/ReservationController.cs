using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Commands.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
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

        [HttpPost("PlaceReservation")]
        public async Task<IActionResult> PlaceReservation()
        {
            var reservationId = ReservationId.New;
            var result = await _commandBus.PublishAsync(new PlaceReservation(reservationId), CancellationToken.None);

            if (result.IsSuccess)
            {
                return Json(reservationId.GetGuid());
            }

            return BadRequest();
        }
    }
}