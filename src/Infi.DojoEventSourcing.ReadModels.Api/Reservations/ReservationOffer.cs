using System;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    [DataContract]
    public class ReservationOffer
    {
        [DataMember]
        public string AggregateId { get; set; } // FIXME ED Rename column to ReservationId

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public DateTime Expires { get; set; }

        [DataMember]
        public decimal Price { get; set; }
    }
}