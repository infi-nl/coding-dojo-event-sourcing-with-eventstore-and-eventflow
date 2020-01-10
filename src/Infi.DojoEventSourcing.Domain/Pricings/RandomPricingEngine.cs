using System;
using Infi.DojoEventSourcing.Domain.Hotels;
using LanguageExt;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Pricings
{
    public class RandomPricingEngine : IPricingEngine
    {
        private const int MaxDaysInFuture = 365;

        public Option<Money> GetAccommodationPrice(DateTime date)
        {
            var limit = DateTime.UtcNow.AddDays(MaxDaysInFuture);

            return date <= limit
                ? new Money(new Random().Next(50, 150), Hotel.Currency)
                : Option<Money>.None;
        }
    }
}