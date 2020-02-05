using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.ReadModels.Api.Rooms.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DojoEventSourcing.Controllers
{
    [Route("[controller]")]
    public class RoomController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public RoomController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

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
    }
}