using System;

namespace Infi.DojoEventSourcing.ReadModels.Api.Rooms.Dtos
{
    public class RoomAvailabilityDto
    {
        public RoomAvailabilityDto(Guid roomId, string roomNumber, bool isAvailable)
        {
            RoomId = roomId;
            RoomNumber = roomNumber;
            IsAvailable = isAvailable;
        }

        public Guid RoomId { get; }
        public string RoomNumber { get; }
        public bool IsAvailable { get; }
    }
}