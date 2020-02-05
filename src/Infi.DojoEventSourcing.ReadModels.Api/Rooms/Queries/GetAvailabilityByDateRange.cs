using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Infi.DojoEventSourcing.Db;
using Infi.DojoEventSourcing.Domain.Rooms.Queries;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms.Queries
{
    public class GetAvailabilityByDateRange : IQuery<IReadOnlyList<RoomAvailabilityDto>>
    {
        public GetAvailabilityByDateRange(DateTime startDateUtc, DateTime endDateUtc)
        {
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
        }

        public DateTime StartDateUtc { get; }
        public DateTime EndDateUtc { get; }
    }

    public class GetAvailabilityByDateRangeHandler
        : IQueryHandler<GetAvailabilityByDateRange, IReadOnlyList<RoomAvailabilityDto>>
    {
        private readonly IDatabaseContext<IApiReadModelRepositoryFactory> _dbReadContext;
        private IReadOnlyList<RoomOccupationReadModel> _roomOccupations;

        public GetAvailabilityByDateRangeHandler(IDatabaseContext<IApiReadModelRepositoryFactory> dbReadContext)
        {
            _dbReadContext = dbReadContext;
        }

        public async Task<IReadOnlyList<RoomAvailabilityDto>> ExecuteQueryAsync(
            GetAvailabilityByDateRange query,
            CancellationToken cancellationToken)
        {
            var rooms = await GetAllRooms();
            _roomOccupations = await GetAllRoomOccupationsFor(query);

            return rooms.Map(room => GetAvailabilityForRoom(query, room)).ToImmutableList();
        }

        private RoomAvailabilityDto GetAvailabilityForRoom(GetAvailabilityByDateRange query, RoomReadModel room)
        {
            var occupiedIntervalsForRoom = _roomOccupations
                .Where(o => o.AggregateId == room.AggregateId)
                .Map(occupation => new RoomAvailabilityIntervalDto
                {
                    Start = occupation.StartDate,
                    End = occupation.EndDate,
                    IsOccupied = true,
                })
                .ToImmutableList();

            var roomAvailabilityIntervals = GetAvailabilityIntervalsForQuery(query, occupiedIntervalsForRoom);

            return new RoomAvailabilityDto
            {
                RoomId = new Guid(room.AggregateId),
                RoomNumber = room.RoomNumber,
                Details = roomAvailabilityIntervals.OrderBy(r => r.Start).ToList(),
                IsAvailable = roomAvailabilityIntervals.Count == 1 && !roomAvailabilityIntervals.Single().IsOccupied,
            };
        }

        private static List<RoomAvailabilityIntervalDto> GetAvailabilityIntervalsForQuery(
            GetAvailabilityByDateRange query,
            ImmutableList<RoomAvailabilityIntervalDto> occupiedIntervalsForRoom)
        {
            var roomAvailabilityIntervals = new List<RoomAvailabilityIntervalDto>(occupiedIntervalsForRoom);
            var previousEnd = query.StartDateUtc;
            foreach (var occupiedInterval in occupiedIntervalsForRoom)
            {
                if (previousEnd < occupiedInterval.Start)
                {
                    roomAvailabilityIntervals.Add(new RoomAvailabilityIntervalDto
                    {
                        Start = previousEnd,
                        End = occupiedInterval.Start,
                        IsOccupied = false,
                    });
                }

                previousEnd = occupiedInterval.End;
            }

            if (previousEnd < query.EndDateUtc)
            {
                roomAvailabilityIntervals.Add(new RoomAvailabilityIntervalDto
                {
                    Start = previousEnd,
                    End = query.EndDateUtc,
                    IsOccupied = false
                });
            }

            return roomAvailabilityIntervals;
        }

        private async Task<IReadOnlyList<RoomOccupationReadModel>> GetAllRoomOccupationsFor(
            GetAvailabilityByDateRange query)
        {
            return await _dbReadContext.RunAsync(factory =>
            {
                var roomRepository = factory.CreateRoomRepository();
                return roomRepository.GetOccupationsBetween(query.StartDateUtc, query.EndDateUtc);
            });
        }

        private async Task<IReadOnlyList<RoomReadModel>> GetAllRooms()
        {
            return await _dbReadContext.RunAsync(factory =>
            {
                var roomRepository = factory.CreateRoomRepository();
                return roomRepository.GetAll();
            });
        }
    }
}