using System;
using System.Runtime.Serialization;

namespace Infi.DojoEventSourcing.Domain.Reservations.Queries
{
    [DataContract]
    public class ReservationOffer
    {
        public ReservationOffer()
        {
        }

        // FIXME Separate database and domain models
        public ReservationOffer(string aggregateId, DateTime date, DateTime expires, decimal price)
        {
            AggregateId = aggregateId;
            Date = date;
            Expires = expires;
            Price = price;
        }

        [DataMember]
        public string AggregateId { get; set; } // FIXME ED Rename column to ReservationId

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public DateTime Expires { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        public bool HasExpired(DateTime date) => !IsStillValid(date);

        public bool IsStillValid(DateTime date) => Expires > date;
    }
}