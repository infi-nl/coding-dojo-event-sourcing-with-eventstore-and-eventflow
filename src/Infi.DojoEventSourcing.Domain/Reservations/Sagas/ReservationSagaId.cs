using EventFlow.Core;
using EventFlow.Sagas;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace Infi.DojoEventSourcing.Domain.Reservations.Sagas
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ReservationSagaId : Identity<ReservationSagaId>, ISagaId
    {
        public ReservationSagaId(string value)
            : base(value)
        {
        }
    }
}