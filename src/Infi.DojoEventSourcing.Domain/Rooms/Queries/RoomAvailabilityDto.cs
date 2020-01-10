using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.Domain.Rooms.Queries
{
    [DataContract]
    public class RoomAvailabilityDto
    {
        [DataMember]
        public Guid RoomId { get; set; }

        [DataMember]
        public string RoomNumber { get; set; }

        [DataMember]
        public bool IsAvailable { get; set; }

        [DataMember]
        public List<RoomAvailabilityIntervalDto> Details { get; set; }
    }
}