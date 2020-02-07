using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Rooms.Commands;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;
using Infi.DojoEventSourcing.ReadModels.Api.Rooms;
using Infi.DojoEventSourcing.ReadModels.Api.Rooms.Queries;
using Microsoft.AspNetCore.Mvc;
using static Infi.DojoEventSourcing.Domain.Rooms.Room;

namespace DojoEventSourcing.Controllers
{
    [Route("[controller]")]
    public class RoomController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;

        public RoomController(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto createRoom)
        {
            var id = RoomIdentity.New;

            var room = await _commandBus
                .PublishAsync(new CreateRoom(id, createRoom.Number), CancellationToken.None)
                .ConfigureAwait(false);

            if (room.IsSuccess)
            {
                return Json(id.GetGuid());
            }

            return BadRequest();
        }

        [HttpGet("GetAllRooms")]
        public async Task<IReadOnlyList<RoomReadModel>> GetAllRooms() =>
            await _queryProcessor
                .ProcessAsync(new GetAllRooms(), CancellationToken.None)
                .ConfigureAwait(false);

        [HttpGet("GetAvailabilityByDateRange")]
        public async Task<IActionResult> GetAvailabilityByDateRange([FromQuery] string startDate, string endDate)
        {
            var parsedStartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var parsedEndDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var availability = await _queryProcessor.ProcessAsync(
                new GetAvailabilityByDateRange(parsedStartDate, parsedEndDate),
                CancellationToken.None);

            return Json(availability);
        }

        public class CreateRoomDto
        {
            public string Number { get; set; }
        }
    }
}