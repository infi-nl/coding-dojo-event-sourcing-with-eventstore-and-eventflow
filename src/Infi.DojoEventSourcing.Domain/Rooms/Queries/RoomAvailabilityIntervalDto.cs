using System;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    [DataContract]
    public class RoomAvailabilityIntervalDto
    {
        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime End { get; set; }

        [DataMember]
        public bool IsOccupied { get; set; }

        public bool OverlapsWith(DateTime start, DateTime end) => start < end && end > start;
    }
}