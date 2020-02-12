using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Domain.Reservations.Queries;
using Infi.DojoEventSourcing.ReadModels.Api.Capacity.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DojoEventSourcing.Controllers
{
    [Route("[controller]")]
    public class CapacityController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public CapacityController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        [HttpGet("{date}")]
        public async Task<CapacityDto> GetByDate(DateTime byDate) =>
            await _queryProcessor.ProcessAsync(new GetCapacityByDate(byDate), CancellationToken.None);

        [HttpGet("{arrival}/{departure}")]
        public async Task<CapacityDto[]> GetByDateRange(DateTime arrival, DateTime departure) =>
            await _queryProcessor.ProcessAsync(new GetCapacityByTimeRange(arrival, departure), CancellationToken.None);
    }
}