using System;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    [DataContract]
    public class CapacityDto
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int Capacity { get; set; }

        [DataMember]
        public int Reserved { get; set; }
    }
}