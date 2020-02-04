using System;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.ReadModels.Api.Reservations
{
    [DataContract]
    public class ReservationOffer
    {
//        public ReservationOffer(string reservationId, DateTime arrival, DateTime departure, decimal totalPrice)
//        {
//            ReservationId = reservationId;
//            Arrival = arrival;
//            Departure = departure;
//            TotalPrice = totalPrice;
//        }

        [DataMember]
        public string ReservationId { get; set; }

        [DataMember]
        public DateTime Arrival { get; set; }

        [DataMember]
        public DateTime Departure { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }
    }
}