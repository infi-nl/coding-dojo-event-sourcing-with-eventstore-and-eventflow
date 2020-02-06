using System;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
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