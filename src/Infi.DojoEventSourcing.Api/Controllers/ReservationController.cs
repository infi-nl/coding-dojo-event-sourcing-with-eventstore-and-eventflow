using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using Infi.DojoEventSourcing.Domain.Commands.Reservations;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace DojoEventSourcing.Controllers
{
    [Route("[controller]")]
    public class ReservationController : Controller
    {
        private readonly ICommandBus _commandBus;

        public ReservationController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
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