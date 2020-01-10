using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace Infi.DojoEventSourcing.Domain.Reservations.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ReservationId : Identity<ReservationId>
    {
        public ReservationId(string value)
            : base(value)
        {
        }
    }
}